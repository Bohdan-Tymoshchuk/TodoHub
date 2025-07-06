using BuildingBlocks.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TodoHub.Application.DTOs;
using TodoHub.Application.Extensions;
using TodoHub.Application.Services;
using TodoHub.Domain;
using TodoHub.Domain.Abstractions;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Contexts;
using TodoHub.Domain.Entities;
using TodoHub.Domain.Exceptions;
using TodoHub.Domain.Pagination;

namespace TodoHub.Tests;

public class TodoCollectionsTests
{
    private readonly Mock<ITodoCollectionRepository> _mockTodoCollectionRepository = new();
    private readonly Mock<ITodoSharedCollectionRepository> _mockTodoSharedCollectionRepository = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IUserContext> _mockUserContext = new();

    private readonly TodoCollectionService _todoCollectionService;
    private readonly TodoSharedCollectionService _todoSharedCollectionService;

    public TodoCollectionsTests()
    {
        _todoCollectionService = new TodoCollectionService(_mockTodoCollectionRepository.Object, _mockUnitOfWork.Object, _mockUserContext.Object);
        _todoSharedCollectionService = new TodoSharedCollectionService(_mockTodoSharedCollectionRepository.Object, _todoCollectionService, _mockUserContext.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateTodoCollectionAsync_ShouldCreateTaskList_WhenValidInput()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new TodoCollectionDto
        {
            Name = "Test Task List",
            Tasks = [
                new TodoTaskDto { Title = "Task 1", Description = "Description 1", IsCompleted = false},
                new TodoTaskDto { Title = "Task 2", Description = "Description 2", IsCompleted = false}
            ]
        };
        
        var createdTaskList = dto.ToEntity(userId);
        
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockTodoCollectionRepository.Setup(x => x.CreateAsync(It.IsAny<TodoCollection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTaskList);

        // Act
        var result = await _todoCollectionService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
        result.Tasks.Count.Should().Be(dto.Tasks.Count);
        _mockTodoCollectionRepository.Verify(x => x.CreateAsync(It.IsAny<TodoCollection>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetTodoCollectionAsync_ShouldThrowNotFoundException_WhenUserHasNoAccess()
    {
        // Arrange
        var todoCollectionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockTodoCollectionRepository.Setup(x => x.GetByIdAsync(todoCollectionId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TodoCollection);

        // Act & Assert
        await _todoCollectionService.Invoking(s => s.GetByIdAsync(todoCollectionId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Entity \"{nameof(TodoCollection)}\" ({todoCollectionId}) was not found.");
    }

    [Fact]
    public async Task DeleteTodoCollectionAsync_ShouldThrowNotFoundException_WhenUserIsNotOwner()
    {
        // Arrange
        var todoCollectionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockTodoCollectionRepository.Setup(x => x.GetByIdForOwnerOnlyAsync(todoCollectionId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TodoCollection);

        // Act & Assert
        await _todoCollectionService.Invoking(s => s.DeleteAsync(todoCollectionId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Entity \"{nameof(TodoCollection)}\" ({todoCollectionId}) was not found.");
    }

    [Fact]
    public async Task ShareTaskListAsync_ShouldThrowBusinessLogicException_WhenMaxSharesReached()
    {
        // Arrange
        var todoCollectionId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var targetUserId = Guid.NewGuid();
        var dto = new TodoSharedCollectionDto() { UserId = targetUserId,  TodoCollectionId = todoCollectionId };
        var todoCollection = new TodoCollection
        {
            Id = todoCollectionId,
            OwnerId = ownerId,
            Name = "Test List"
        };
        
        _mockUserContext.Setup(x => x.UserId).Returns(ownerId);
        _mockTodoCollectionRepository.Setup(x => x.GetByIdAsync(todoCollectionId, ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todoCollection);
        _mockTodoSharedCollectionRepository.Setup(x => x.GetAsync(todoCollectionId, It.IsAny<CancellationToken>()))!
            .ReturnsAsync([
                new TodoSharedCollection { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), TodoCollectionId = todoCollectionId },
                new TodoSharedCollection { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), TodoCollectionId = todoCollectionId },
                new TodoSharedCollection { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), TodoCollectionId = todoCollectionId }
            ]);

        // Act & Assert
        await _todoSharedCollectionService.Invoking(s => s.ShareTodoCollectionAsync(dto, It.IsAny<CancellationToken>()))
            .Should().ThrowAsync<BadRequestException>()
            .WithMessage($"You can only share a collection with {Constants.ShareTodoCollectionLimit} users at a time.");
    }

    [Fact]
    public async Task GetUserTaskListsAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskLists = new List<TodoCollection>
        {
            new() { Id = Guid.NewGuid(), Name = "List 1", OwnerId = userId, CreatedDate = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "List 2", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-1) },
            new() { Id = Guid.NewGuid(), Name = "List 3", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-2) },
            new() { Id = Guid.NewGuid(), Name = "List 4", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-3) },
            new() { Id = Guid.NewGuid(), Name = "List 5", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-4) },
            new() { Id = Guid.NewGuid(), Name = "List 6", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-5) },
            new() { Id = Guid.NewGuid(), Name = "List 7", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-6) },
            new() { Id = Guid.NewGuid(), Name = "List 8", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-7) },
            new() { Id = Guid.NewGuid(), Name = "List 9", OwnerId = userId, CreatedDate = DateTime.UtcNow.AddMinutes(-8) },
        };
        
        var paginatedResult = new PaginatedResult<TodoCollection>(1, 5, taskLists.Count, taskLists.Take(5).ToList());
        var paginationRequest = new PaginationRequest(1, 5);
        
        _mockUserContext.Setup(x => x.UserId).Returns(userId);
        _mockTodoCollectionRepository.Setup(x => x.GetAllAsync(userId, paginationRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedResult);

        // Act
        var result = await _todoCollectionService.GetAllAsync(paginationRequest, It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(taskLists.Count);
        result.PageIndex.Should().Be(1);
        result.PageSize.Should().Be(5);
    }
}