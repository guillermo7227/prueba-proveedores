using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using Swashbuckle.AspNetCore.Annotations;

namespace Proveedores.API
{
    /// <summary>
    /// Parámetros para una petición de inicio de sesión
    /// </summary>
    public class LoginRequest
    {
        /// <example>alfredo@gmail.com</example>
        [SwaggerSchema("Email del usuario")]
        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Email { get; set; }
        
        /// <example>MiP@55w0rd123+.</example>
        [SwaggerSchema("Contraseña del usuario")]
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }
    }
}