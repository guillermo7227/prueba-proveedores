using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Proveedores.API.Models;
using Proveedores.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Proveedores.API.Controllers
{

    [SwaggerTag("Lee, crea, actualiza y borra Proveedores")]
    [Authorize(Roles = "admin,usuario")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : Controller
    {
        private IProveedorService _proveedorService { get; set; }

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        private ProveedorDto makeDto(Proveedor registro)
        {
            var dto = new ProveedorDto()
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                Activo = registro.Activo,
                Email = registro.Email,
                NIT = registro.NIT,
                Telefono = registro.Telefono,
            };

            return dto;
        }

        [SwaggerOperation(
            Summary = "Obtiene todos los proveedores",
            Description = "Obtiene lista de todos los proveedores existentes en base de datos",
            OperationId = "GetAllProveedor",
            Tags = new[] { "Proveedor" }
        )]
        [SwaggerResponse(200, "Lista de proveedores", typeof(APIResponse<List<ProveedorDto>>))]
        [SwaggerResponse(500, "Error interno al obtener los proveedores", typeof(ProblemDetails))]
        [HttpGet]
        public async Task<ActionResult<APIResponse<object>>> GetAll()
        {
            var response = new APIResponse<object>();
            try
            {
                var registros = await _proveedorService.GetAll();
                
                var registrosDto = registros.Select(x => makeDto(x)).ToList();

                response.Data = registros;
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }

        [SwaggerOperation(
            Summary = "Obtiene un proveedor según su id",
            Description = "Obtiene un proveedor basado en su id",
            OperationId = "GetByIdProveedor",
            Tags = new[] { "Proveedor" }
        )]
        [SwaggerResponse(200, "Registro de Proveedor encontrado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(404, "Proveedor no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al obtener el proveedor", typeof(ProblemDetails))]
        [HttpGet("{id}")]
        public async Task<ActionResult<APIResponse<object>>> GetById([SwaggerParameter("Id proveedor", Required = true)]string id)
        {
            var response = new APIResponse<object>();

            try
            {
                var registro = await _proveedorService.GetById(id);
                
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

        [SwaggerOperation(
            Summary = "Crea un proveedor",
            Description = "Crea un nuevo proveedor con los datos ingresados",
            OperationId = "AddProveedor",
            Tags = new[] { "Proveedor" }
        )]
        [SwaggerResponse(200, "Registro de Proveedor creado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(500, "Error interno al crear el proveedor", typeof(ProblemDetails))]
        [HttpPost]
        public async Task<ActionResult<APIResponse<object>>> Add([SwaggerParameter("Datos proveedor", Required = true)]Proveedor proveedor)
        {
            var response = new APIResponse<object>();

            try
            {
                var registroExistente = await _proveedorService.GetByNIT(proveedor.NIT);

                if(registroExistente != null)
                {
                    ModelState.AddModelError("NIT", "NIT ya está registrado");
                    return ValidationProblem(ModelState);
                }

                proveedor.Id = ObjectId.GenerateNewId().ToString();
                var result = await _proveedorService.Add(proveedor);

                if(result != true)
                {
                    return Problem("Error al crear registro");
                }
                
                response.Status = (int)HttpStatusCode.Created;
                response.Detail = "Creado satisfactoriamente";
                response.Data = makeDto(proveedor);
                return response;         
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }

        }


        [SwaggerOperation(
            Summary = "Actualiza un proveedor",
            Description = "Actualiza un proveedor existente con los datos ingresados",
            OperationId = "UpdateProveedor",
            Tags = new[] { "Proveedor" }
        )]
        [SwaggerResponse(200, "Registro de Proveedor actualizado", typeof(APIResponse<ProveedorDto>))]
        [SwaggerResponse(400, "Error en validación de argumentos", typeof(HttpValidationProblemDetails))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el proveedor", typeof(ProblemDetails))]
        [HttpPut]
        public async Task<ActionResult<APIResponse<object>>> Update([SwaggerParameter("Id proveedor", Required = true)]string id, [SwaggerParameter("Datos del proveedor", Required = true)]Proveedor proveedor)
        {
            var response = new APIResponse<object>();

            try
            {  
                if(id != proveedor.Id)
                {
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Detail = "El id de la ruta debe coincidir con el id del cuerpo de la petición";
                    return BadRequest(response);
                }

                var registro = await _proveedorService.GetById(id);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }

                if(registro.NIT != proveedor.NIT)
                {

                    var nitExistente = await _proveedorService.GetByNIT(proveedor.NIT);

                    if(nitExistente != null)
                    {
                        ModelState.AddModelError("NIT", "NIT ya está registrado");
                        return ValidationProblem(ModelState);
                    }
                }

                var result = await _proveedorService.Update(id, proveedor);
                
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


        [SwaggerOperation(
            Summary = "Elimina un proveedor",
            Description = "Elimina un proveedor existente según el id ingresado",
            OperationId = "DeleteProveedor",
            Tags = new[] { "Proveedor" }
        )]
        [SwaggerResponse(200, "Registro de Proveedor eliminado", typeof(APIResponse<object>))]
        [SwaggerResponse(404, "Registro no encontrado", typeof(APIResponse<object>))]
        [SwaggerResponse(500, "Error interno al crear el proveedor", typeof(ProblemDetails))]
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse<object>>> Delete([SwaggerParameter("Id proveedor. Ej: 6728438f2f464d00ee9ae80c", Required = true)]string id)
        {
            var response = new APIResponse<object>();

            try
            {  
                var registro = await _proveedorService.GetById(id);
                
                if(registro == null)
                {
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Detail = "No se encontró registro";
                    return NotFound(response);
                }
                var result = await _proveedorService.Delete(id);
                
                if(result != true)
                {
                    return Problem("Error al eliminar registro");
                }
                
                response.Status = (int)HttpStatusCode.NoContent;
                response.Detail = "Registro eliminado";
                return response;
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);            
            }

        }

    }

}