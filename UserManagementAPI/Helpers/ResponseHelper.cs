using UserManagementAPI.DTOs;

namespace UserManagementAPI.Helpers
{
    public class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(T data, string message = "Acción exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> Fail<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message
            };
        }
    }
}
