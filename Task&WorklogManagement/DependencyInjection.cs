using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task_WorklogManagement.Interfaces;
using Task_WorklogManagement.Interfaces.Security;
using Task_WorklogManagement.Persistence;
using Task_WorklogManagement.Repositories;
using Task_WorklogManagement.Security;
using Task_WorklogManagement.Services;

namespace Task_WorklogManagement
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<RefreshTokenRepository>();
            services.AddScoped<TaskItemRepository>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskItemService, TaskItemService>();

            services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IDbConnectionFactory, NpgsqlConnectionFactory>();
            return services;
        }
    }
}
