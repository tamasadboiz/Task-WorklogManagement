using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domain.Entities;

namespace Task_WorklogManagement.Infrastructure.Persistence
{
    public class TaskWorklogDbContext : DbContext
    {
        public TaskWorklogDbContext(DbContextOptions<TaskWorklogDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<Worklog> Worklogs => Set<Worklog>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(r =>
            {
                r.ToTable("roles");
                r.HasKey(x => x.RoleId);

                r.Property(x => x.RoleId).HasColumnName("role_id");
                r.Property(x => x.RoleName).HasColumnName("role_name").IsRequired();

                r.HasIndex(x => x.RoleName).IsUnique();

                r.HasData(
                    new Role { RoleId = 1, RoleName = "ADMIN" },
                    new Role { RoleId = 2, RoleName = "LEADER" },
                    new Role { RoleId = 3, RoleName = "MEMBER" }
                );
            });

            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("users");
                u.HasKey(x => x.UserId);

                u.Property(x => x.UserId).HasColumnName("user_id");
                u.Property(x => x.Email).HasColumnName("email").IsRequired();
                u.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
                u.Property(x => x.RoleId).HasColumnName("role_id").IsRequired();
                u.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
                u.Property(x => x.UpdatedAt).HasColumnName("updated_at");

                u.HasIndex(x => x.Email).IsUnique();

                u.HasOne<Role>()
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskItem>(t =>
            {
                t.ToTable("task_items");
                t.HasKey(x => x.TaskItemId);

                t.Property(x => x.TaskItemId).HasColumnName("task_item_id");
                t.Property(x => x.Title).HasColumnName("title").IsRequired();
                t.Property(x => x.Description).HasColumnName("description");
                t.Property(x => x.AssigneeId).HasColumnName("assignee_id").IsRequired();
                t.Property(x => x.Deadline).HasColumnName("deadline").IsRequired();
                t.Property(x => x.Priority).HasColumnName("priority").IsRequired();
                t.Property(x => x.Status).HasColumnName("status").IsRequired();
                t.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
                t.Property(x => x.UpdatedAt).HasColumnName("updated_at");

                t.HasIndex(x => new { x.AssigneeId, x.Deadline });
                t.HasIndex(x => x.Status);

                t.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Worklog>(w =>
            {
                w.ToTable("worklos");
                w.HasKey(x => x.WorklogId);

                w.Property(x => x.WorklogId).HasColumnName("worklog_id");
                w.Property(x => x.TaskItemId).HasColumnName("task_item_id").IsRequired();
                w.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
                w.Property(x => x.WorkDate).HasColumnName("work_date").IsRequired(); ;
                w.Property(x => x.HoursSpent).HasColumnName("hours_spent").IsRequired();
                w.Property(x => x.Note).HasColumnName("note");
                w.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

                w.HasIndex(x => new { x.TaskItemId, x.UserId, x.WorkDate }).IsUnique();
                w.ToTable(t => t.HasCheckConstraint("ck_worklogs_hoursspent", "hours_spent > 0 AND  hours_spent <= 8"));

                w.HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .OnDelete(DeleteBehavior.Restrict);

                w.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefreshToken>(r =>
            {
                r.ToTable("refresh_tokens");
                r.HasKey(x => x.RefreshTokenId);

                r.Property(x => x.RefreshTokenId).HasColumnName("refresh_token_id");
                r.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
                r.Property(x => x.Token).HasColumnName("token").IsRequired();
                r.Property(x => x.ExpiresAt).HasColumnName("expires_at").IsRequired();
                r.Property(x => x.RevokedAt).HasColumnName("revoked_at");
                r.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");

                r.HasIndex(x => x.Token).IsUnique();
                r.HasIndex(x => new { x.UserId, x.ExpiresAt });

                r.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
