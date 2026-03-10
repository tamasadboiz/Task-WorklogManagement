using System.Net;
using System.Text.Json;
using Task_WorklogManagement.Models;

namespace Task_WorklogManagement.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ArgumentException ex)
            {
                await HandleException(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException ex)
            {
                await HandleException(context, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, ex.Message, HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                await HandleException(context, "Đã xảy ra lỗi hệ thống.", HttpStatusCode.InternalServerError);
            }
        }

        private static async Task HandleException(HttpContext context, string message, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var response = ResponseModel.Error(message, statusCode);
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
