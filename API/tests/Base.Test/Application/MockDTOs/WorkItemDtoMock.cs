using Moq;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.CQRS.Queries;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;

namespace TaskingSystem.Tests.Application.MockDTOs
{
    public static class WorkItemDtoMock
    {
        public static WorkItemDto GetMockWorkItemDto()
        {
            return new WorkItemDto
            {
                Id = Guid.NewGuid(),
                Title = "Task Title",
                Description = "Task Description",
                DueDate = DateTime.UtcNow.AddDays(10),
                IdUser = Guid.NewGuid(),
                User = "John Doe",
                Active = true,
                IdStatus = Guid.NewGuid(),
                Status = "In Progress",
                DateCreated = DateTime.UtcNow.AddDays(-1),
                IdUserCreated = Guid.NewGuid(),
                UserCreated = "Admin",
                DateUpdated = DateTime.UtcNow,
                IdUserUpdated = Guid.NewGuid(),
                UserUpdated = "Admin"
            };
        }

        public static Mock<IQueryDispatcher> GetMockQueryDispatcher(ApiResponse<WorkItemDto> apiResponse)
        {
            var mockQueryDispatcher = new Mock<IQueryDispatcher>();

            mockQueryDispatcher
                .Setup(q => q.Dispatch<GetWorkItemByIdQuery, ApiResponse<WorkItemDto>>(It.IsAny<GetWorkItemByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            return mockQueryDispatcher;
        }

        public static ApiResponse<WorkItemDto> GetMockApiResponse(bool success, WorkItemDto workItemDto = null)
        {
            return new ApiResponse<WorkItemDto>
            {
                Success = success,
                Data = workItemDto ?? GetMockWorkItemDto()
            };
        }
    }

}
