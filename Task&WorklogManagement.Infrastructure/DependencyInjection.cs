using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Application.Interfaces.Security;
using Task_WorklogManagement.Infrastructure.Persistence;
using Task_WorklogManagement.Infrastructure.Security;

namespace Task_WorklogManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IDbConnectionFactory, NpgsqlConnectionFactory>();
            return services;
        }
    }
}
