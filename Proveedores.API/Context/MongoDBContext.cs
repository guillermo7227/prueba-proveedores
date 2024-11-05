using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Proveedores.API.Models;

namespace Proveedores.API.Context
{
    public class MongoDBContext : DbContext
    {
        public DbSet<Proveedor> Proveedores { get; set; }    
        public DbSet<Usuario> Usuarios { get; set; }    

        public MongoDBContext(DbContextOptions options) : base(options)
        {            
        }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Proveedor>().ToCollection("proveedor");
            builder.Entity<Usuario>().ToCollection("usuario");
        }
    }

}