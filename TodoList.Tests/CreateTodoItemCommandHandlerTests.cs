using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using TodoList.Application.CreateTodoItem;
using TodoList.Domain.TodoItem;
using MediatR;

[TestFixture]
public class CreateTodoItemCommandHandlerTests
{
    private Mock<ITodoItemRepository> _repositoryMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ITodoItemRepository>();
    }

    [TearDown]
    public void TearDown()
    {
        _repositoryMock = null;
    }

    [Test]
    public async Task Should_Create_TodoItem_Successfully()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Test Todo Item",
            Description = "Test Description"
        };
        var handler = new CreateTodoItemCommandHandler(_repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(result.Title, command.Title);
        Assert.AreEqual(result.Description, command.Description);

        _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Should_Sanitize_Input_Successfully()
    {
        var expectedOutput = "alert(xss)";
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "<script>alert('xss')</script>",
            Description = "<script>alert('xss')</script>"
        };
        var handler = new CreateTodoItemCommandHandler(_repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verifying that HTML tags, potentially dangerous SQL characters and XSS attack attempts have been removed
        Assert.AreEqual(expectedOutput, result.Title);
        Assert.AreEqual(expectedOutput, result.Description);

        _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Should_Truncate_Long_Input_Successfully()
    {
        // Arrange
        var longInput = new string('a', 1500);
        var expectedOutput = new string('a', 1000);
        var command = new CreateTodoItemCommand
        {
            Title = longInput,
            Description = longInput
        };
        var handler = new CreateTodoItemCommandHandler(_repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verifying that input longer than 1000 characters has been truncated
        Assert.AreEqual(expectedOutput, result.Title);
        Assert.AreEqual(expectedOutput, result.Description);

        _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public void Should_Not_Throw_Exception_For_Too_Long_Description()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Valid Title",
            Description = new string('a', 1001)
        };
        var handler = new CreateTodoItemCommandHandler(_repositoryMock.Object);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await handler.Handle(command, CancellationToken.None));
    }
}
