using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> GetAll();

        Task<Proveedor?> GetById(string id);

        Task<Proveedor?> GetByNIT(Int64 nit);

        Task<bool> Add(Proveedor proveedor);

        Task<bool> Update(string id, Proveedor proveedor);

        Task<bool> Delete(string id);
    }
}