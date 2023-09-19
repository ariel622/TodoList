namespace TodoList.Domain.TodoItem
{
    public interface ITodoItemRepository
    {
        Task<TodoItem> GetTodoItemByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);
        Task RemoveTodoItemAsync(int id, CancellationToken cancellationToken);
        Task UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);

    }
}