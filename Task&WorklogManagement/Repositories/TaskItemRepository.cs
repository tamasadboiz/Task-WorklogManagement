using Dapper;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Persistence;

namespace Task_WorklogManagement.Repositories
{
    public class TaskItemRepository
    {
        private readonly IDbConnectionFactory _factory;
        public TaskItemRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            const string sql = @"SELECT task_item_id as TaskItemId,
                                        title as Title,
                                        description as Description,
                                        assignee_id as AssigneeId,
                                        deadline as Deadline,
                                        priority as Priority,
                                        status as Status,
                                        created_at as CreatedAt,
                                        updated_at as UpdatedAt
                                        FROM task_items
                                        ORDER BY created_at DESC";
            using var conn = _factory.CreateConnection();
            var result = await conn.QueryAsync<TaskItem>(sql);
            return result.ToList();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            const string sql = @"SELECT task_item_id as TaskItemId,
                                        title as Title,
                                        description as Description,
                                        assignee_id as AssigneeId,
                                        deadline as Deadline,
                                        priority as Priority,
                                        status as Status,
                                        created_at as CreatedAt,
                                        updated_at as UpdatedAt
                                        FROM task_items
                                        WHERE task_item_id = @Id
                                        LIMIT 1";

            using var conn = _factory.CreateConnection();
            var result = await conn.QueryFirstOrDefaultAsync<TaskItem>(sql, new { Id = id });
            return result;
        }

        public async Task CreateAsync(TaskItem taskItem)
        {
            const string sql = @"INSERT INTO task_items (task_item_id, title, description, assignee_id, deadline, priority, status, created_at)
                                VALUES (@TaskItemId, @Title, @Description, @AssigneeId, @Deadline, @Priority, @Status, now())";

            using var conn = _factory.CreateConnection();

            await conn.ExecuteAsync(sql, taskItem);
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            const string sql = @"UPDATE task_items
                                SET title = @Title, 
                                    description = @Description, 
                                    assignee_id = @AssigneeId, 
                                    deadline = @Deadline, 
                                    priority = @Priority, 
                                    status = @Status, 
                                    updated_at = now()
                                WHERE task_item_id = @TaskItemId";

            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, taskItem);
        }    

        public async Task DeleteAsync(Guid id)
        {
            const string sql = @"DELETE FROM task_items
                                WHERE task_item_id = @Id";

            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
