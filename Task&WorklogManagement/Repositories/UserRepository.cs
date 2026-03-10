using Dapper;
using System.Data;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Persistence;

namespace Task_WorklogManagement.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnectionFactory _factory;
        public UserRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        //CHECK EXIST EMAIL
        public async Task<bool> ExistByEmailAsync(string email)
        {
            const string sql = @"SELECT 1 FROM USERS WHERE EMAIL = @EMAIL LIMIT 1;";
            using var conn = _factory.CreateConnection();
            var found = await conn.ExecuteScalarAsync<int?>(sql, new { Email = email });
            return found.HasValue;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"SELECT
                                    u.user_id as UserId,
                                    u.email as Email,
                                    u.password_hash as PasswordHash,
                                    u.role_id as RoleId,
                                    u.created_at as CreatedAt,
                                    u.updated_at as UpdatedAt,
                                    r.role_id as RoleRoleId,
                                    r.role_name as RoleName
                                FROM users u JOIN roles r ON r.role_id = u.role_id
                                WHERE u.email = @Email
                                LIMIT 1;";
            using var conn = _factory.CreateConnection();
            var result = await conn.QueryAsync<User, Role, User>(
                sql, 
                (u, r) => { u.Role = r; return u; }, 
                new { Email = email }, 
                splitOn: "RoleRoleId"
            );
            return result.FirstOrDefault();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            const string sql = @"SELECT 
                                    u.user_id as UserId,
                                    u.email as Email,
                                    u.role_id as RoleId,
                                    u.created_at as CreatedAt,
                                    u.updated_at as UpdatedAt,
                                    r.role_id as RoleRoleId,
                                    r.role_name as RoleName
                                FROM users u JOIN roles r ON r.role_id = u.role_id
                                WHERE u.user_id = @UserId
                                LIMIT 1;";
            using var conn = _factory.CreateConnection();
            var result = await conn.QueryAsync<User, Role, User>(
                sql,
                (u, r) => { u.Role = r; return u; },
                new { UserId = userId },
                splitOn: "RoleRoleId"
            );
            return result.FirstOrDefault();
        }

        public async Task<List<User>> GetAllAsync()
        {
            const string sql = @"SELECT 
                                    u.user_id as UserId,
                                    u.email as Email,
                                    u.role_id as RoleId,
                                    u.created_at as CreatedAt,
                                    u.updated_at as UpdatedAt,
                                    r.role_id as RoleRoleId,
                                    r.role_name as RoleName
                                FROM users u JOIN roles r ON r.role_id = u.role_id
                                ORDER BY u.created_at DESC";

            using var conn = _factory.CreateConnection();

            var lookup = new Dictionary<Guid, User>();

            var rows = await conn.QueryAsync<User, Role, User>(
                sql,
                (u, r) =>
                {
                    if (!lookup.TryGetValue(u.UserId, out var existed))
                    {
                        existed = u;
                        lookup.Add(existed.UserId, existed);
                    }
                    existed.Role = r;
                    return existed;
                },
                splitOn: "RoleRoleId"
            );
            return new List<User>(lookup.Values);
        }

        public async Task CreateAsync(User user)
        {
            const string sql = @"INSERT INTO users(user_id, email, password_hash, role_id, created_at)
                                 VALUES (@UserId, @Email, @PasswordHash, @RoleId, now());";
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, user);
        }

        public async Task CreateAsync(User user, IDbConnection conn, IDbTransaction transaction )
        {
            const string sql = @"INSERT INTO users(user_id, email, password_hash, role_id, created_at)
                                 VALUES (@UserId, @Email, @PasswordHash, @RoleId, now());";
            await conn.ExecuteAsync(sql, user, transaction);
        }

        public async Task UpdateAsync(User user)
        {
            const string sql = @"UPDATE users 
                                 SET email = @EMAIL,
                                     password_hash = @PasswordHash,
                                     role_id = @RoleId,
                                     updated_at = now()
                                 WHERE user_id = @UserId;";
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            const string sql = @"DELETE FROM users WHERE user_id = @UserId;";
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}
