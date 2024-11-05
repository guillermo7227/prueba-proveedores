using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAll();

        Task<Usuario?> GetById(string id);
        
        Task<Usuario?> GetByEmail(string email);

        Task<bool> Add(Usuario Usuario);

        Task<bool> Update(string id, UsuarioUpdateRequest Usuario);

        Task<bool> Delete(string id);

        Task<bool> ChangePassword(string id, string password);
    }
}