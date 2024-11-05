// using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Proveedores.API.Context;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly MongoDBContext _mongodbContext;

        public ProveedorService(MongoDBContext mongodbContext)
        {
            _mongodbContext = mongodbContext;
        }


        public async Task<List<Proveedor>> GetAll()
        {
            return await _mongodbContext.Proveedores.ToListAsync();
        }

        public async Task<Proveedor?> GetById(string id)
        {
            return await _mongodbContext.Proveedores.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Proveedor?>  GetByNIT(Int64 nit)
        {
            return await _mongodbContext.Proveedores.FirstOrDefaultAsync(x => x.NIT == nit);
        }

        public async Task<bool> Add(Proveedor proveedor)
        {
            await _mongodbContext.Proveedores.AddAsync(proveedor);
            _mongodbContext.SaveChanges();

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var proveedor = await _mongodbContext.Proveedores.FirstOrDefaultAsync(x => x.Id == id);

            _mongodbContext.Proveedores.Remove(proveedor);
            _mongodbContext.SaveChanges();

            return true;
        }

        public async Task<bool> Update(string id, Proveedor proveedor)
        {
            var proveedorBD = await _mongodbContext.Proveedores.FirstOrDefaultAsync(x => x.Id == id);

            proveedorBD.Activo = proveedor.Activo;
            proveedorBD.NIT = proveedor.NIT;
            proveedorBD.Email = proveedor.Email;
            proveedorBD.Nombre = proveedor.Nombre;
            proveedorBD.Telefono = proveedor.Telefono;
            
            _mongodbContext.Update(proveedorBD);
            _mongodbContext.SaveChanges();

            return true;
        }
    }
}