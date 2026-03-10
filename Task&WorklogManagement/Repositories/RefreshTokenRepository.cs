using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Domains.Entities;
using Task_WorklogManagement.Persistence;

namespace Task_WorklogManagement.Repositories
{
    public class RefreshTokenRepository
    {
        private readonly IDbConnectionFactory _factory;
        public RefreshTokenRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            const string sql = @"INSERT INTO refresh_tokens(refresh_token_id, user_id, token, expires_at, revoked_at, created_at)
                                 VALUES (@RefreshTokenId, @UserId, @Token, @ExpiresAt, @RevokedAt, now())";

            using var conn= _factory.CreateConnection();
            await conn.ExecuteAsync(sql, refreshToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
        {
            const string sql = @"SELECT
                                    refresh_token_id as RefreshTokenId,
                                    user_id as UserId,
                                    token as Token,
                                    expires_at as ExpiresAt,
                                    revoked_at as RevokedAt.
                                    created_at as CreatedAt
                                 FROM refresh_tokens
                                 WHERE token = @Token
                                 LIMIT 1;";
            using var conn = _factory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { Token = refreshToken });
        }

        public async Task RevokeAsync(string refreshToken)
        {
            const string sql = @"UPDATE refresh_tokens
                                 SET revoked_at = now()
                                 WHERE token = @Token and revoked_at is null;";
            using var conn = _factory.CreateConnection();
            await conn.ExecuteAsync(sql, new { Token = refreshToken });
        }
    }
}
