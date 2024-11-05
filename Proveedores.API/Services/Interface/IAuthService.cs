using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public interface IAuthService
    {

        Task<AuthResponse> Login(LoginRequest login);
    }
}