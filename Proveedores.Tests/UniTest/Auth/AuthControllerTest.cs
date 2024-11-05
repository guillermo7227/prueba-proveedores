using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using Proveedores.API;
using Proveedores.API.Controllers;
using Proveedores.API.Models;
using Proveedores.API.Services;

namespace Proveedores.Tests.UnitTest;

public class AuthControllerTest
{
    [Fact]
    public async Task Auth_WhenLogin_ShouldGiveToken()
    {
        LoginRequest loginRequest = new LoginRequest() 
        {
            Email = "superuser@example.com",
            Password = "123+."
        };

        AuthResponse authResponse = new AuthResponse()
        {
            Nombre = "Super User",
            Email = "superuser@example.com",
            Token = "el-token-1234"
        };

        Usuario usuario = new Usuario()
        {
            Id = "6728438f2f464d00ee9ae80c",
            Nombre = "Super User",
            Activo = true,
            Roles = "admin",
            Email = "superuser@example.com"
        };

        SuperUserSettings suSettings = new SuperUserSettings()
        {
            Email = "superuser@example.com",
            Password = "123+.",
            Nombre = "Super User",
            Roles = "admin"
        };

        Mock<IAuthService> _mockAuthService = new Mock<IAuthService>();
        _mockAuthService.Setup(x => x.Login(loginRequest)).Returns(Task.FromResult(authResponse));

        Mock<IUsuarioService> _mockUsuarioService = new Mock<IUsuarioService>(); 
        _mockUsuarioService.Setup(x => x.GetByEmail(It.IsAny<string>())).Returns(Task.FromResult(usuario));

        Mock<IUtilService> _mockUtilService = new Mock<IUtilService>(); 
        _mockUtilService.Setup(x => x.VerifyPassword(It.IsAny<string>(),It.IsAny<string>())).Returns(true);

        Mock<IOptions<SuperUserSettings>> _mockIOptions = new Mock<IOptions<SuperUserSettings>>(); 
        _mockIOptions.Setup(x => x.Value).Returns(suSettings);

        var authController = new AuthController(_mockAuthService.Object,_mockUsuarioService.Object,_mockUtilService.Object,_mockIOptions.Object);

        var result =  await authController.Login(loginRequest);

        Assert.IsType<ActionResult<APIResponse<object>>>(result);
        Assert.IsType<APIResponse<object>>(result.Value);
        
        var authResponseData = result.Value.Data as AuthResponse;
        Assert.IsType<AuthResponse>(authResponseData);

        Assert.Equal("el-token-1234", authResponse.Token);
    }
}