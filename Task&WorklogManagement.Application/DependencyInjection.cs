using Microsoft.Extensions.DependencyInjection;


namespace Task_WorklogManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
