using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Presentation.Controllers;

namespace TaskingSystem.Tests.Presentation
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IQueryDispatcher> _mockQueryDispatcher;
        private Mock<ICommandDispatcher> _mockCommandDispatcher;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockCommandDispatcher = new Mock<ICommandDispatcher>();
            _userController = new UserController(_mockQueryDispatcher.Object, _mockCommandDispatcher.Object);
        }

        #region Create - POST
        [Test]
        public async Task CreateUser_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                FullName = "John Doe",
                UserName = "johndoe",
                Password = "password123",
                IdRole = Guid.NewGuid()
            };

            var response = new ApiResponse<Guid>
            {
                Success = true,
                Data = Guid.NewGuid()
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<CreateUserCommand, ApiResponse<Guid>>(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.CreateUser(command, CancellationToken.None);

            // Assert
            var okResult = result as CreatedAtActionResult;
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task CreateUser_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                FullName = "",
                UserName = "",
                Password = "short",
                IdRole = Guid.Empty
            };

            var response = new ApiResponse<Guid>
            {
                Success = false,
                ErrorMessage = "One or more validation errors occurred.",
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<CreateUserCommand, ApiResponse<Guid>>(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.CreateUser(command, CancellationToken.None);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<Guid>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("One or more validation errors occurred.", apiResponse.ErrorMessage);
        }
        #endregion

        #region Update - PUT
        [Test]
        public async Task UpdateUser_IdsDoNotMatch_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = Guid.NewGuid(), // Distinct ID
                FullName = "John Doe"
            };

            // Act
            var result = await _userController.UpdateUser(id, command, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task UpdateUser_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = id,
                FullName = "John Doe"
            };

            var response = new ApiResponse<bool>
            {
                Success = true,
                Data = true
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<UpdateUserCommand, ApiResponse<bool>>(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.UpdateUser(id, command, CancellationToken.None);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<bool>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse.Success);
        }

        [Test]
        public async Task UpdateUser_CommandFails_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdateUserCommand
            {
                Id = id,
                FullName = "John Doe"
            };

            var response = new ApiResponse<bool>
            {
                Success = false,
                ErrorMessage = "Update failed"
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<UpdateUserCommand, ApiResponse<bool>>(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.UpdateUser(id, command, CancellationToken.None);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<bool>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Update failed", apiResponse.ErrorMessage);
        }
        #endregion

        #region GetById - GET
        [Test]
        public async Task GetUserById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetUserByIdQuery { IdUser = id };
            var response = new ApiResponse<UserDto>
            {
                Success = true,
                Data = new UserDto { Id = id, FullName = "John Doe" }
            };

            _mockQueryDispatcher
                .Setup(x => x.Dispatch<GetUserByIdQuery, ApiResponse<UserDto>>(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.GetUserById(id, CancellationToken.None);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<UserDto>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse.Success);
            Assert.AreEqual("John Doe", apiResponse.Data.FullName);
        }

        [Test]
        public async Task GetUserById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetUserByIdQuery { IdUser = id };
            var response = new ApiResponse<UserDto>
            {
                Success = false,
                ErrorMessage = "User not found"
            };

            _mockQueryDispatcher
                .Setup(x => x.Dispatch<GetUserByIdQuery, ApiResponse<UserDto>>(It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.GetUserById(id, CancellationToken.None);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<UserDto>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("User not found", apiResponse.ErrorMessage);
        }
        #endregion

        #region DELETE
        [Test]
        public async Task DeleteUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new ApiResponse<bool>
            {
                Success = true,
                Data = true
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<DeleteUserCommand, ApiResponse<bool>>(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.DeleteUser(id, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteUser_CommandFails_ReturnsBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new ApiResponse<bool>
            {
                Success = false,
                ErrorMessage = "Deletion failed"
            };

            _mockCommandDispatcher
                .Setup(x => x.Dispatch<DeleteUserCommand, ApiResponse<bool>>(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _userController.DeleteUser(id, CancellationToken.None);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var apiResponse = badRequestResult.Value as ApiResponse<bool>;
            Assert.IsNotNull(apiResponse);
            Assert.IsFalse(apiResponse.Success);
            Assert.AreEqual("Deletion failed", apiResponse.ErrorMessage);
        } 
        #endregion
    }
}
