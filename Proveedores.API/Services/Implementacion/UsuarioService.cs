using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Proveedores.API.Context;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly MongoDBContext _mongodbContext;
        private readonly IUtilService _utilService;

        public UsuarioService(MongoDBContext mongodbContext, IUtilService utilService)
        {
            _mongodbContext = mongodbContext;
            _utilService = utilService;
        }


        public async Task<List<Usuario>> GetAll()
        {
            return await _mongodbContext.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetById(string id)
        {
            return await _mongodbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Usuario?>  GetByEmail(string email)
        {
            return await _mongodbContext.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> Add(Usuario usuario)
        {
            await _mongodbContext.Usuarios.AddAsync(usuario);
            _mongodbContext.SaveChanges();

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var usuario = await _mongodbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            _mongodbContext.Usuarios.Remove(usuario);
            _mongodbContext.SaveChanges();

            return true;
        }

        public async Task<bool> Update(string id, UsuarioUpdateRequest usuario)
        {
            var usuarioBD = await _mongodbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            // se puede usar Map
            // https://jasonwatmore.com/post/2022/01/16/net-6-hash-and-verify-passwords-with-bcrypt
            usuarioBD.Activo = usuario.Activo;
            usuarioBD.Email = usuario.Email;
            usuarioBD.Nombre = usuario.Nombre;
            usuarioBD.Roles = usuario.Roles;

            _mongodbContext.Update(usuarioBD);
            _mongodbContext.SaveChanges();

            return true;
        }

        public async Task<bool> ChangePassword([FromRoute] string id, [FromBody] string password)
        {
            var usuario = await _mongodbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            
            usuario.Password = _utilService.HashPassword(password);

            _mongodbContext.Update(usuario);
            _mongodbContext.SaveChanges();

            return true;
        }

    }
}