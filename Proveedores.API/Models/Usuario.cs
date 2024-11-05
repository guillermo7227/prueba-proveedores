using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Proveedores.API.Models
{
    /// <summary>
    /// Modelo de datos para un Usuario
    /// </summary>
    public class Usuario
    {
        /// <example>6728438f2f464d00ee9ae80c</example>
        [SwaggerSchema("Id del usuario")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "Debe ingresar un Id")]
        public string? Id { get; set; }

        /// <example>Alfredo Areyanes</example>
        [SwaggerSchema("Nombre del usuario")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
    
        /// <example>alfredo@gmail.com</example>
        [SwaggerSchema("Email del usuario")]
        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Email { get; set; }

        /// <example>MiP@55w0rd123+.</example>
        [SwaggerSchema("Contraseña del usuario", Format = "email")]
        [Required(ErrorMessage = "Debe ingresar una contraseña")] 
        public string Password { get; set; }
        
        /// <example>true</example>
        [SwaggerSchema("Estado del usuario")]
        public bool Activo { get; set; } = true;

        /// <example>usuario,analista</example>
        [SwaggerSchema("Roles del usuario (separados por coma)")]
        public string? Roles { get; set; } = "usuario";
               
    }

    /// <summary>
    /// Modelo de vista de un Usuario
    /// </summary>
    public class UsuarioDto
    {

        /// <example>6728438f2f464d00ee9ae80c</example>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <example>Alfredo Areyanes</example>
        public string Nombre { get; set; }
    
        /// <example>alfredo@gmail.com</example>
        public string Email { get; set; }
        
        /// <example>true</example>
        public bool Activo { get; set; }

        /// <example>usuario,analista</example>
        public string? Roles { get; set; }
    }
    
    /// <summary>
    /// Parámetros para actualizar un usuario.
    /// </summary>    
    public class UsuarioUpdateRequest
    {

        /// <example>6728438f2f464d00ee9ae80c</example>
        [SwaggerSchema("Id del usuario")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "Debe ingresar un Id")]
        public string? Id { get; set; }

        /// <example>Alfredo Areyanes</example>
        [SwaggerSchema("Nombre del usuario")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
    
        /// <example>alfredo@gmail.com</example>
        [SwaggerSchema("Email del usuario")]
        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Email { get; set; }
        
        /// <example>true</example>
        [SwaggerSchema("Estado del usuario")]
        public bool Activo { get; set; } = true;
        
        /// <example>usuario,analista</example>
        [SwaggerSchema("Roles del usuario (separados por coma)")]
        public string? Roles { get; set; } = "usuario";
    }
}