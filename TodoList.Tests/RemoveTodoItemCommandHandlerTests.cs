using Moq;
using TodoList.Application.RemoveTodoItem;
using TodoList.Domain.TodoItem;

[TestFixture]
public class RemoveTodoItemCommandHandlerTests
{
    private Mock<ITodoItemRepository> _repositoryMock;
    private RemoveTodoItemCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ITodoItemRepository>();
        _handler = new RemoveTodoItemCommandHandler(_repositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _repositoryMock = null;
        _handler = null;
    }

    [Test]
    public async Task Should_Remove_TodoItem_Successfully()
    {
        // Arrange
        var command = new RemoveTodoItemCommand(1);

        var todoItem = new TodoItem();
        _repositoryMock.Setup(r => r.GetTodoItemByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(todoItem);
        _repositoryMock.Setup(r => r.RemoveTodoItemAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.RemoveTodoItemAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }


    [Test]
    public async Task Should_Throw_Exception_For_Non_Existent_TodoItem()
    {
        // Arrange
        var command = new RemoveTodoItemCommand(999);
        _repositoryMock.Setup(r => r.RemoveTodoItemAsync(999, It.IsAny<CancellationToken>())).ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        Assert.ThrowsAsync<TodoList.Application.Exceptions.NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Should_Throw_Exception_For_Invalid_Id()
    {
        // Arrange
        var command = new RemoveTodoItemCommand(-1);

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
    }

}
