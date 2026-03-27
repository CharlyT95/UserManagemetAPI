using UserManagementAPI.DTOs;

namespace UserManagementAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                var response = new ApiResponse<object>
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
