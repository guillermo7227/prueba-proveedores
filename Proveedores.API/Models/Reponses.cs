namespace Proveedores.API.Models
{
    /// <summary>
    /// Respuesta para una petición
    /// </summary>
    public class APIResponse<T>
    {
        /// <example>200</example>
        public int Status { get; set; } = 200;

        /// <example>Resultado de la petición</example>
        public string Title { get; set; } = "Resultado de la petición";

        /// <example>Completado</example>
        public string Detail { get; set; } = "Completado";

        /// <example>Objeto de datos (esquema: Proveedor)</example>
        public T? Data { get; set; }
    }

    /// <summary>
    /// Respuesta para una petición de autenticación
    /// </summary>
    public class AuthResponse
    {
        /// <example>alfredo@gmail.com</example>
        public string Email { get; set; }

        /// <example>Alfredo Areyanes</example>
        public string Nombre { get; set; }

        /// <example>eyJhbGciOiJIE3MzA3MzkwNTcsImV4cCI6MTczMDc0OTg1NywiaWF0IjoxNzMwNzM5MDU3LCJpc3MiOiJodHRwczovL2xvY2</example>
        public string Token { get; set; }
    }
}