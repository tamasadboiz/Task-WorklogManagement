using AutoMapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_WorklogManagement.Infrastructure.Persistence
{
    public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        public NpgsqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection CreateConnection()
        {
            var cs = _config.GetConnectionString("PostgreSqlConnection");
            return new NpgsqlConnection(cs);
        }
    }
}
