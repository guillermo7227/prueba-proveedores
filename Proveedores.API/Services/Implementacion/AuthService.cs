// using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Proveedores.API.Context;
using Proveedores.API.Models;

namespace Proveedores.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly MongoDBContext _mongodbContext;
        private readonly IUsuarioService _usuarioService;
        public readonly IConfiguration _configuration;
        public readonly IUtilService _utilService;

        public AuthService(MongoDBContext mongodbContext, IUsuarioService usuarioService, IConfiguration configuration, IUtilService utilService)
        {
            _mongodbContext = mongodbContext;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _utilService = utilService;
        }

        public async Task<AuthResponse> Login(LoginRequest login)
        {
            var usuario = await _usuarioService.GetByEmail(login.Email);

            string tokenString = CreateToken(usuario);

            var AuthResponse = new AuthResponse() {
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Token = tokenString
            };

            return AuthResponse;
        }

        public string CreateToken(Usuario user)
        {
            var jwtSettings = _configuration.GetSection("JWtSettings").Get<JWTSettings>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var subjectClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email)
            };

            var roles  = (user.Roles ?? "").Split(',');
            foreach (var rol in roles)
            {
                subjectClaims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(subjectClaims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(jwtSettings.TokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.ValidIssuer, // Add this line
                Audience = jwtSettings.ValidAudience 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}