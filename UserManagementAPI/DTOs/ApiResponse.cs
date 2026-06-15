namespace UserManagementAPI.DTOs
{
    public class ApiResponse<T>
    {
        public bool Resultado { get; set; }
        public T Datos { get; set; }
        public string Mensaje { get; set; }

    }
}
