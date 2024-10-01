using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Presentation.Controllers;
using TaskingSystem.Presentation.Dispatcher;
using TaskingSystem.Tests.Application.MockDTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskingSystem.Tests.Presentation
{
    [TestFixture]
    public class WorkItemControllerTests
    {
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IQueryDispatcher> _queryDispatcherMock;
        private WorkItemController _controller;

        [SetUp]
        public void Setup()
        {
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _queryDispatcherMock = WorkItemDtoMock.GetMockQueryDispatcher(WorkItemDtoMock.GetMockApiResponse(true));

            _controller = new WorkItemController(_queryDispatcherMock.Object, _commandDispatcherMock.Object);

            // Mock de HttpContext y HttpResponse para simular Response.Headers
            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();
            mockResponse.Setup(r => r.Headers).Returns(headers);
            mockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
        }

        [Test]
        public async Task CreateWorkItem_ReturnsCreatedResult_WhenSuccess()
        {
            // Arrange
            var command = new CreateWorkItemCommand { Title = "New Task" };
            var response = new ApiResponse<Guid> { Success = true, Data = Guid.NewGuid() };
            _commandDispatcherMock
                .Setup(x => x.Dispatch<CreateWorkItemCommand, ApiResponse<Guid>>(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateTask(command, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public async Task CreateWorkItem_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var command = new CreateWorkItemCommand { Title = "New Task" };
            var response = new ApiResponse<Guid> { Success = false };
            _commandDispatcherMock
                .Setup(x => x.Dispatch<CreateWorkItemCommand, ApiResponse<Guid>>(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateTask(command, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateWorkItem_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var command = new UpdateWorkItemCommand { Id = Guid.NewGuid(), Title = "Updated Task" };
            var response = new ApiResponse<bool> { Success = true };
            _commandDispatcherMock
                .Setup(x => x.Dispatch<UpdateWorkItemCommand, ApiResponse<bool>>(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateTask(command.Id, command, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateWorkItem_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var command = new UpdateWorkItemCommand { Id = Guid.NewGuid(), Title = "Updated Task" };

            // Act
            var result = await _controller.UpdateTask(Guid.NewGuid(), command, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task GetWorkItemById_ReturnsOkResult_WhenSuccess()
        {
            var workItemDto = WorkItemDtoMock.GetMockWorkItemDto();
            _queryDispatcherMock = WorkItemDtoMock.GetMockQueryDispatcher(WorkItemDtoMock.GetMockApiResponse(true, workItemDto));

            var result = await _controller.GetTaskById(workItemDto.Id, CancellationToken.None);

            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var apiResponse = okResult.Value as ApiResponse<WorkItemDto>;
            Assert.IsNotNull(apiResponse);
            Assert.IsTrue(apiResponse.Success);

            // Verificamos que los valores del WorkItemDto retornado coincidan
            var returnedWorkItem = apiResponse.Data;
            Assert.AreEqual(workItemDto.Title, returnedWorkItem.Title);
            Assert.AreEqual(workItemDto.Description, returnedWorkItem.Description);
        }

        [Test]
        public async Task GetWorkItemById_ReturnsBadRequest_WhenNotSuccess()
        {
            var response = new ApiResponse<WorkItemDto> { Success = false, Data = null };

            _queryDispatcherMock
                .Setup(x => x.Dispatch<GetWorkItemByIdQuery, ApiResponse<WorkItemDto>>(It.IsAny<GetWorkItemByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.GetTaskById(Guid.NewGuid(), CancellationToken.None);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Get_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var query = new GetAllWorkItemsQuery();
            var pagedList = new PagedList<WorkItemDto>(new List<WorkItemDto>(), 0, 1, 10);
            var response = new ApiResponse<PagedList<WorkItemDto>> { Success = true, Data = pagedList };

            _queryDispatcherMock
                .Setup(x => x.Dispatch<GetAllWorkItemsQuery, ApiResponse<PagedList<WorkItemDto>>>(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Get(query, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var query = new GetAllWorkItemsQuery();
            var response = new ApiResponse<PagedList<WorkItemDto>> { Success = false };

            _queryDispatcherMock
                .Setup(x => x.Dispatch<GetAllWorkItemsQuery, ApiResponse<PagedList<WorkItemDto>>>(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Get(query, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteWorkItem_ReturnsOkResult_WhenSuccess()
        {
            // Arrange
            var response = new ApiResponse<bool> { Success = true };

            _commandDispatcherMock
                .Setup(x => x.Dispatch<DeleteWorkItemCommand, ApiResponse<bool>>(It.IsAny<DeleteWorkItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTask(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteWorkItem_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var response = new ApiResponse<bool> { Success = false };

            _commandDispatcherMock
                .Setup(x => x.Dispatch<DeleteWorkItemCommand, ApiResponse<bool>>(It.IsAny<DeleteWorkItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteTask(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
