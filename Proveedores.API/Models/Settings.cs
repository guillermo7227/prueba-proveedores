using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Proveedores.API.Models
{
    public class MongoDBSettings
    {
       public string DatabaseURI { get; set; }       
       public string DatabaseName { get; set; }       
                
    }

    public class JWTSettings
    {
       public string ValidAudience { get; set; }       
       public string ValidIssuer { get; set; }       
       public int TokenExpiryTimeInHour { get; set; }       
       public string Secret { get; set; }       
                
    }

    public class SuperUserSettings
    {
       public string Email { get; set; }       
       public string Nombre { get; set; }       
       public string Password { get; set; }       
       public string Roles { get; set; }       
                
    }
}