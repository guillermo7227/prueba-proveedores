using System.Net;
using System.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Proveedores.API.Models;
using Proveedores.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Proveedores.API
{
    [SwaggerTag("Acciones de autenticación (login)")]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUsuarioService _usuarioService;
        private IUtilService _utilService;

        public AuthController(IAuthService authService, IUsuarioService usuarioService, IUtilService utilService, IOptions<SuperUserSettings> suSettings)
        {
            _authService = authService;
            _usuarioService = usuarioService;
            _utilService = utilService;
            CreateSuperUser(suSettings.Value);
        }

        private async void CreateSuperUser(SuperUserSettings suSettings)
        {
            var registro = await _usuarioService.GetByEmail(suSettings.Email);

            if(registro == null)
            {
                try
                {
                    var superuser = new Usuario() 
                    {
                        Email = suSettings.Email,
                        Nombre = suSettings.Nombre,
                        Password = _utilService.HashPassword(suSettings.Password),
                        Activo = true,
                        Roles = suSettings.Roles,
                        Id = ObjectId.GenerateNewId().ToString()
                    };
                    var result = await _usuarioService.Add(superuser);

                    if(result != true)
                    {
                        throw new Exception();
                    }
                    
                }
                catch (System.Exception)
                {
                    throw new SecurityException("No se pudo crear el superusuario");
                }
            }
        }

        [SwaggerOperation(
            Summary = "Autenticar usuario en el sistema",
            Description = "Otorga acceso a los endpoints de la API mediante un token JWT",
            OperationId = "LoginAuth",
            Tags = new[] { "Auth" }
        )]
        [SwaggerResponse(200, "Acceso otorgado", typeof(APIResponse<AuthResponse>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(401, "Usuario está en estado inactivo", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el proveedor", typeof(ProblemDetails))]
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<APIResponse<object>>> Login(LoginRequest login)
        {
            var response = new APIResponse<object>();
            try
            {
                var usuario = await _usuarioService.GetByEmail(login.Email);

                if(usuario == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró el usuario";
                    return NotFound(response);
                }

                if(usuario.Activo == false)
                {
                    response.Status= (int)HttpStatusCode.Unauthorized;
                    response.Detail = "El usuario está inactivo";
                    return Unauthorized(response);
                }

                if (!_utilService.VerifyPassword(usuario.Password, login.Password))
                {
                    response.Status= (int)HttpStatusCode.BadRequest;
                    response.Detail = "La contraseña es incorrecta";
                    return BadRequest(response);
                }
                
                AuthResponse loginResult = await _authService.Login(login);

                response.Data = loginResult;
                return response;            
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }

    }
}