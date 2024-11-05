using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Proveedores.API.Models
{
    /// <summary>
    /// Modelo de datos para un Proveedor
    /// </summary>
    public class Proveedor
    {
        /// <example>6728438f2f464d00ee9ae80c</example>
        [SwaggerSchema("Id del proveedor")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "Debe ingresar un Id")]
        public string? Id { get; set; }

        /// <example>Ferretería La Veloz</example>
        [SwaggerSchema("Nombre del proveedor")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
    
        /// <example>123456789</example>
        [SwaggerSchema("NIT/CC/CE del proveedor")]
        [Required(ErrorMessage = "Debe ingresar un NIT")]
        public Int64 NIT { get; set; }

        /// <example>+573017774455</example>
        [SwaggerSchema("Teléfono del proveedor")]
        public string? Telefono { get; set; }

        /// <example>laveloz@gmail.com</example>
        [SwaggerSchema("Email del proveedor")]
        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
        public string Email { get; set; }

        /// <example>true</example>
        [SwaggerSchema("Estado del proveedor")]
        [Required(ErrorMessage = "Debe ingresar un estado")]
        public bool Activo { get; set; } = true;        
                
    }


    /// <summary>
    /// Modelo de vista de un Proveedor
    /// </summary>
    public class ProveedorDto
    {
        /// <example>6728438f2f464d00ee9ae80c</example>
        public string? Id { get; set; }

        /// <example>Ferretería La Veloz</example>
        public string Nombre { get; set; }

        /// <example>123456789</example>
        public Int64 NIT { get; set; }

        /// <example>+573017774455</example>
        public string? Telefono { get; set; }

        /// <example>laveloz@gmail.com</example>
        public string Email { get; set; }

        /// <example>true</example>
        public bool Activo { get; set; }       
                
    }
}