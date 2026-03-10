using Microsoft.Extensions.DependencyInjection;
using Task_WorklogManagement.Application.Interfaces;
using Task_WorklogManagement.Application.Services;


namespace Task_WorklogManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
