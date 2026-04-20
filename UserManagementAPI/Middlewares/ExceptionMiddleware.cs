using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (DbUpdateException ex)
            {
                await HandleDatabaseException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericException(context, ex);
            }
        }

        private async Task HandleDatabaseException(HttpContext context, DbUpdateException ex)
        {
            string message = "Error en base de datos";
            if (ex.InnerException is SqlException sqlEx)
            {
                if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                {
                    message = "Registro duplicado";
                    context.Response.StatusCode = 400;
                }
                else
                {
                    context.Response.StatusCode = 500;
                }
            }
            else
            {
                context.Response.StatusCode = 500;
            }

            var response = new ApiResponse<object>
            {
                Success = false,
                Data = null,
                Message = message
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private async Task HandleGenericException(HttpContext context, Exception ex)
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
