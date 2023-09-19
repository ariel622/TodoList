using TodoList.Domain.TodoItem;

namespace TodoList.Infrastructure.Database
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> GetTodoItemByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.TodoItems.FindAsync(id, cancellationToken);
        }

        public async Task CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveTodoItemAsync(int id, CancellationToken cancellationToken)
        {
            var todoItem = new TodoItem { Id = id };
            _context.TodoItems.Attach(todoItem);
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
