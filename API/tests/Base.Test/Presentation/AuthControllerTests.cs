using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Presentation.Controllers;

namespace TaskingSystem.Tests.Presentation
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<ICommandDispatcher> _mockCommandDispatcher;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            // Inicializamos el mock del ICommandDispatcher
            _mockCommandDispatcher = new Mock<ICommandDispatcher>();

            // Creamos una instancia del controlador pasando los mocks configurados
            _controller = new AuthController(null, _mockCommandDispatcher.Object);
        }

        [Test]
        public async Task Authenticate_ReturnsOkResult_WhenSuccess()
        {
            // Arrange: Configuramos el mock para que devuelva un resultado exitoso
            var authCommand = new AuthCommand { UserName = "user", Password = "password" };
            var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "user", Token = "mock_token" };

            var apiResponse = new ApiResponse<UserDto> { Success = true, Data = userDto };
            _mockCommandDispatcher
                .Setup(x => x.Dispatch<AuthCommand, ApiResponse<UserDto>>(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            // Act: Llamamos al método Authenticate del controlador
            var result = await _controller.Authenticate(authCommand, CancellationToken.None);

            // Assert: Verificamos que el resultado sea OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);

            // Verificamos que el valor devuelto sea el ApiResponse esperado
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedApiResponse = okResult.Value as ApiResponse<UserDto>;
            Assert.IsNotNull(returnedApiResponse);
            Assert.IsTrue(returnedApiResponse.Success);

            // Verificamos que los valores del UserDto retornado coincidan
            Assert.AreEqual(userDto.UserName, returnedApiResponse.Data.UserName);
            Assert.AreEqual(userDto.Token, returnedApiResponse.Data.Token);
        }

        [Test]
        public async Task Authenticate_ReturnsBadRequest_WhenNotSuccess()
        {
            // Arrange: Configuramos el mock para que devuelva una respuesta no exitosa
            var authCommand = new AuthCommand { UserName = "user", Password = "wrong_password" };

            var apiResponse = new ApiResponse<UserDto> { Success = false };
            _mockCommandDispatcher
                .Setup(x => x.Dispatch<AuthCommand, ApiResponse<UserDto>>(It.IsAny<AuthCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            // Act: Llamamos al método Authenticate del controlador con datos inválidos
            var result = await _controller.Authenticate(authCommand, CancellationToken.None);

            // Assert: Verificamos que el resultado sea BadRequestObjectResult
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
