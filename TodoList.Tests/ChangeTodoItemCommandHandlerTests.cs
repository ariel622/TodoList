using NUnit.Framework;
using System.Threading.Tasks;
using TodoList.Infrastructure.Database;
using Moq;
using TodoList.Domain.TodoItem;
using TodoList.Application.ChangeTodoItem;
using TodoList.Application.Exceptions;

[TestFixture]
public class ChangeTodoItemCommandHandlerTests
{
    private Mock<ITodoItemRepository> _repositoryMock;
    private ChangeTodoCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ITodoItemRepository>();
        _handler = new ChangeTodoCommandHandler(_repositoryMock.Object);
    }

    [Test]
    public async Task Should_Update_TodoItem_Successfully()
    {
        // Arrange
        int existingItemId = 1;
        string newTitle = "Updated Title";
        string newDescription = "Updated Description";
        bool newIsIncomplete = false;

        var existingItem = new TodoItem
        {
            Id = existingItemId,
            Title = "Old Title",
            Description = "Old Description",
            IsIncomplete = true
        };

        _repositoryMock.Setup(repo => repo.GetTodoItemByIdAsync(existingItemId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingItem);
        _repositoryMock.Setup(repo => repo.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        var command = new ChangeTodoItemCommand(existingItemId, newTitle, newDescription, newIsIncomplete);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(newTitle, result.Title);
        Assert.AreEqual(newDescription, result.Description);
        Assert.AreEqual(newIsIncomplete, result.IsIncomplete);
    }

    [Test]
    public async Task Should_Sanitize_Input_Successfully()
    {
        // Arrange
        int existingItemId = 1;
        string newTitle = "New Title<script>alert('xss');</script>";
        string newDescription = "New Description<script>alert('xss');</script>";
        bool newIsIncomplete = false;

        var existingItem = new TodoItem
        {
            Id = existingItemId,
            Title = "Old Title",
            Description = "Old Description",
            IsIncomplete = true
        };

        _repositoryMock.Setup(repo => repo.GetTodoItemByIdAsync(existingItemId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingItem);
        _repositoryMock.Setup(repo => repo.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        var command = new ChangeTodoItemCommand(existingItemId, newTitle, newDescription, newIsIncomplete);

        // The expected sanitized title and description
        string expectedSanitizedTitle = "New Titlealert(xss)";
        string expectedSanitizedDescription = "New Descriptionalert(xss)";

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(expectedSanitizedTitle, result.Title);
        Assert.AreEqual(expectedSanitizedDescription, result.Description);
        Assert.AreEqual(newIsIncomplete, result.IsIncomplete);
    }



    [Test]
    public async Task Should_Partially_Update_TodoItem_Successfully()
    {
        // Arrange
        int existingItemId = 1;
        string newTitle = "Updated Title";

        var existingItem = new TodoItem
        {
            Id = existingItemId,
            Title = "Old Title",
            Description = "Old Description",
            IsIncomplete = true
        };

        _repositoryMock.Setup(repo => repo.GetTodoItemByIdAsync(existingItemId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingItem);
        _repositoryMock.Setup(repo => repo.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        var command = new ChangeTodoItemCommand(existingItemId, newTitle, null, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(newTitle, result.Title);
        Assert.AreEqual(existingItem.Description, result.Description);
        Assert.AreEqual(existingItem.IsIncomplete, result.IsIncomplete);
    }

    [Test]
    public void Should_Throw_NotFoundException_For_NonExistent_Item()
    {
        // Arrange
        int nonExistentItemId = 99;
        string newTitle = "New Title";

        _repositoryMock.Setup(repo => repo.GetTodoItemByIdAsync(nonExistentItemId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((TodoItem)null);

        var command = new ChangeTodoItemCommand(nonExistentItemId, newTitle, null, null);

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Should_Sanitize_Long_Input_Successfully()
    {
        // Arrange
        int existingItemId = 1;
        string newTitle = new string('A', 1100); // Creating a too long title

        var existingItem = new TodoItem
        {
            Id = existingItemId,
            Title = "Old Title",
            Description = "Old Description",
            IsIncomplete = true
        };

        _repositoryMock.Setup(repo => repo.GetTodoItemByIdAsync(existingItemId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingItem);
        _repositoryMock.Setup(repo => repo.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask);

        var command = new ChangeTodoItemCommand(existingItemId, newTitle, null, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.AreEqual(new string('A', 1000), result.Title); // Assert that the title has been truncated to 1000 chars
    }
}
