using System.Net;
using System.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Proveedores.API.Models;
using Proveedores.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Proveedores.API.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private IUsuarioService _usuarioService { get; set; }
        public IUtilService _utilService { get; set; }

        public UsuarioController(IUsuarioService usuarioService, IUtilService utilService)
        {
            _usuarioService = usuarioService;
            _utilService = utilService;
        }


        private UsuarioDto makeDto(Usuario registro)
        {
            var dto = new UsuarioDto()
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                Activo = registro.Activo,
                Email = registro.Email,
                Roles = registro.Roles,
            };

            return dto;
        }

        /// <summary>
        ///  Obtiene todos los usuarios registrados
        /// </summary>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Obtiene la lista de usuarios registrados")]
        [SwaggerResponse(200, "Registros consultados", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<APIResponse<object>>> GetAll()
        {
            var response = new APIResponse<object>();
            try
            {
                var registros = await _usuarioService.GetAll();    
                            
                var registrosDto = registros.Select(x => makeDto(x)).ToList();

                response.Data = registrosDto;
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }

        /// <summary>
        ///  Obtiene un usuario según su id
        /// </summary>
        /// <param name="id" example="6728438f2f464d00ee9ae80c">Id del usuario</param>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Obtiene un usuario registrados por su id")]
        [SwaggerResponse(200, "Registro encontrado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<object>>> GetById(string id)
        {
            var response = new APIResponse<object>();

            try
            {
                var registro = await _usuarioService.GetById(id);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }

                response.Data = makeDto(registro);
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }
        }

        /// <summary>
        ///  Obtiene un usuario según su email
        /// </summary>
        /// <param name="email" example="alfredo@gmail.com">Email del usuario</param>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Obtiene un usuario registrado por su email")]
        [SwaggerResponse(200, "Registro encontrado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [Route("find-by-email/{email}")]
        [HttpGet]
        public async Task<ActionResult<APIResponse<object>>> GetByEmail([FromRoute] string email)
        {
            var response = new APIResponse<object>();

            try
            {
                if(!_utilService.IsValidEmail(email))
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Detail = "El email ingresado debe ser un email válido";
                    return BadRequest(response);
                }

                var registro = await _usuarioService.GetByEmail(email);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }

                response.Data = makeDto(registro);
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }

        /// <summary>
        ///  Crea un usuario
        /// </summary>
        /// <param name="usuario">Datos del nuevo usuario (esquema: Usuario)</param>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Crea un nuevo usuario según los datos ingresados")]
        [SwaggerResponse(201, "Registro creado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [HttpPost]
        public async Task<ActionResult<APIResponse<object>>> Add(Usuario usuario)
        {
            var response = new APIResponse<object>();

            try
            {
                if(!_utilService.IsValidPassword(usuario.Password))
                {
                    ModelState.AddModelError("Password", "La contraseña debe cumplir requisitos de seguridad");
                    return ValidationProblem(ModelState);
                }
              
                var registroExistente = await _usuarioService.GetByEmail(usuario.Email);

                if(registroExistente != null)
                {
                    ModelState.AddModelError("Email", "Email ya está registrado");
                    return ValidationProblem(ModelState);
                }

                usuario.Id = ObjectId.GenerateNewId().ToString();

                usuario.Password = _utilService.HashPassword(usuario.Password);
                
                var result = await _usuarioService.Add(usuario);

                if(result != true)
                {
                    return Problem("Error al crear registro");
                }
                
                response.Status = (int)HttpStatusCode.Created;
                response.Detail = "Creado satisfactoriamente";
                response.Data = makeDto(usuario);
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        /// <summary>
        ///  Actualiza un usuario
        /// </summary>
        /// <param name="id" example="6728438f2f464d00ee9ae80c">Id del usuario</param>
        /// <param name="usuario">Datos del usuario (esquema: UsuarioUpdateRequest)</param>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Actualiza un usuario registrado")]
        [SwaggerResponse(200, "Registro actualizado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse<object>>> Update([FromRoute] string id, [FromBody] UsuarioUpdateRequest usuario)
        {
            var response = new APIResponse<object>();

            try
            {
                if(id != usuario.Id)
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Detail = "El id de la ruta debe coincidir con el id del cuerpo de la petición";
                    return BadRequest(response);
                }

                var registro = await _usuarioService.GetById(id);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }

                var result = await _usuarioService.Update(id, usuario);

                if(result != true)
                {
                    return Problem("Error al actualizar registro");
                }
                
                response.Data = makeDto(registro);
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }


        /// <summary>
        ///  Cambia la contraseña de un usuario
        /// </summary>
        /// <param name="id" example="6728438f2f464d00ee9ae80c">Id del usuario</param>
        /// <param name="password">Nueva contraseña (esquema: ChangePasswordRequest)</param>
        /// <returns>APIResponse</returns>
        [SwaggerOperation(Description = "Cambia la contraseña de un usuario registros")]
        [SwaggerResponse(200, "Registro eliminado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el registro", typeof(ProblemDetails))]
        [Route("change-password/{id}")]
        [HttpPost]
        public async Task<ActionResult<APIResponse<object>>> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordRequest password)
        {
            var response = new APIResponse<object>();

            try
            {
                if(id != password.Id)
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Detail = "El id de la ruta debe coincidir con el id del cuerpo de la petición";
                    return BadRequest(response);
                }

                if(!_utilService.IsValidPassword(password.Password))
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Detail = "La contraseña debe cumplir requisitos de seguridad";
                    return BadRequest(response);
                }
              
                var registro = await _usuarioService.GetById(id);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }

                var result = await _usuarioService.ChangePassword(id, password.Password);

                if(result != true)
                {
                    return Problem("Error al actualizar registro");
                }
                
                response.Data = makeDto(registro);
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }
    }
}