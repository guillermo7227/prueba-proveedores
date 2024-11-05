using System.Net;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using Proveedores.API;
using Proveedores.API.Controllers;
using Proveedores.API.Models;
using Proveedores.API.Services;

namespace Proveedores.Tests.UnitTest;

public class ProveedorControllerTest
{
    [Fact]
    public async Task Proveedor_ShouldGetAll()
    {

        List<Proveedor> listaProveedor = new List<Proveedor>();
        listaProveedor.Add(new Proveedor() 
            { 
                Id = "6728438f2f464d00ee9ae80c",
                Nombre = "Ferretería La Veloz",
                NIT = 123456789,
                Telefono = "+573017774455",
                Email = "laveloz@gmail.com",
                Activo = true
            }
        );

        Mock<IProveedorService> _mockProveedorService = new Mock<IProveedorService>(); 
        _mockProveedorService.Setup(x => x.GetAll()).Returns(Task.FromResult(listaProveedor));

        var proveedorController = new ProveedorController(_mockProveedorService.Object);

        var result =  await proveedorController.GetAll();

        Assert.IsType<ActionResult<APIResponse<object>>>(result);
        Assert.IsType<APIResponse<object>>(result.Value);
        
        var apiResponseData = result.Value.Data as List<Proveedor>;
        Assert.IsType<List<Proveedor>>(apiResponseData);

        Assert.Equal("6728438f2f464d00ee9ae80c", listaProveedor.First().Id);

    }

    
    [Fact]
    public async Task Proveedor_ShouldGetById()
    {

        Proveedor proveedor = new Proveedor() 
        { 
            Id = "6728438f2f464d00ee9ae80c",
            Nombre = "Ferretería La Veloz",
            NIT = 123456789,
            Telefono = "+573017774455",
            Email = "laveloz@gmail.com",
            Activo = true
        };

        Mock<IProveedorService> _mockProveedorService = new Mock<IProveedorService>(); 
        _mockProveedorService.Setup(x => x.GetById(It.IsAny<string>())).Returns(Task.FromResult(proveedor));

        var proveedorController = new ProveedorController(_mockProveedorService.Object);

        var result =  await proveedorController.GetById(It.IsAny<string>());

        Assert.IsType<ActionResult<APIResponse<object>>>(result);
        Assert.IsType<APIResponse<object>>(result.Value);
        
        var apiResponseData = result.Value.Data as ProveedorDto;
        Assert.IsType<ProveedorDto>(apiResponseData);

        Assert.Equal("6728438f2f464d00ee9ae80c", apiResponseData.Id);
    }

    
    [Fact]
    public async Task Proveedor_ShouldUpdate()
    {

        Proveedor proveedor = new Proveedor() 
        { 
            Id = "6728438f2f464d00ee9ae80c",
            Nombre = "Ferretería La Veloz",
            NIT = 123456789,
            Telefono = "+573017774455",
            Email = "laveloz@gmail.com",
            Activo = true
        };

        Proveedor proveedorActualizado = new Proveedor() 
        { 
            Id = "6728438f2f464d00ee9ae80c",
            Nombre = "Ferretería La Veloz Modificado",
            NIT = 123456789,
            Telefono = "+573017774455",
            Email = "laveloz@gmail.com",
            Activo = true
        };

        Mock<IProveedorService> _mockProveedorService = new Mock<IProveedorService>(); 
        _mockProveedorService.Setup(x => x.GetById(It.IsAny<string>())).Returns(Task.FromResult(proveedor));
        _mockProveedorService.Setup(x => x.Update(It.IsAny<string>(), proveedorActualizado)).Returns(Task.FromResult(true));

        var proveedorController = new ProveedorController(_mockProveedorService.Object);

        var result =  await proveedorController.Update("6728438f2f464d00ee9ae80c", proveedorActualizado);

        Assert.IsType<ActionResult<APIResponse<object>>>(result);
        Assert.IsType<APIResponse<object>>(result.Value);
        
        var apiResponseData = result.Value.Data as ProveedorDto;
        Assert.IsType<ProveedorDto>(apiResponseData);

        Assert.Equal("6728438f2f464d00ee9ae80c", apiResponseData.Id);
    }
    
    [Fact]
    public async Task Proveedor_ShouldDelete()
    {

        Proveedor proveedor = new Proveedor() 
        { 
            Id = "6728438f2f464d00ee9ae80c",
            Nombre = "Ferretería La Veloz",
            NIT = 123456789,
            Telefono = "+573017774455",
            Email = "laveloz@gmail.com",
            Activo = true
        };

        Mock<IProveedorService> _mockProveedorService = new Mock<IProveedorService>(); 
        _mockProveedorService.Setup(x => x.GetById(It.IsAny<string>())).Returns(Task.FromResult(proveedor));
        _mockProveedorService.Setup(x => x.Delete(It.IsAny<string>())).Returns(Task.FromResult(true));

        var proveedorController = new ProveedorController(_mockProveedorService.Object);

        var result =  await proveedorController.Delete("6728438f2f464d00ee9ae80c");

        Assert.IsType<ActionResult<APIResponse<object>>>(result);
        Assert.IsType<APIResponse<object>>(result.Value);

        Assert.Equal("Registro eliminado", result.Value.Detail);
        Assert.Equal((int)HttpStatusCode.NoContent, result.Value.Status);
    }
}