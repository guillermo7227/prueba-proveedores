using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace Proveedores.API
{
    /// <summary>
    /// Parámetros para cambio de contraseña
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <example>6728438f2f464d00ee9ae80c</example>
        [SwaggerSchema("Id del usuario")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "Debe ingresar un Id")]
        public string Id { get; set; }
        
        /// <example>MiP@55w0rd123+.</example>
        [SwaggerSchema("Contraseña del usuario")]
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        public string Password { get; set; }
    }
}