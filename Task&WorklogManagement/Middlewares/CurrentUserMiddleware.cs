using System.Security.Claims;

namespace Task_WorklogManagement.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;
        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

                context.Items["UserId"] = userId;
                context.Items["Email"] = email;
                context.Items["Role"] = role;
            }
            await _next(context);
        }
    }
}
