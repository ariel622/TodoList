using MediatR;
using TodoList.Domain.TodoItem;
using TodoList.Application.Exceptions;
using TodoList.Application.Sanitization;

namespace TodoList.Application.ChangeTodoItem
{
    public class ChangeTodoCommandHandler : IRequestHandler<ChangeTodoItemCommand, TodoItem>
    {
        private readonly ITodoItemRepository _repository;

        public ChangeTodoCommandHandler(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoItem> Handle(ChangeTodoItemCommand request, CancellationToken cancellationToken)
        {
            request.Title = InputSanitizer.SanitizeInput(request.Title);
            request.Description = InputSanitizer.SanitizeInput(request.Description);

            var todoItem = await _repository.GetTodoItemByIdAsync(request.Id, cancellationToken);
            if (todoItem == null)
            {
                throw new NotFoundException($"Todo Item with Id '{request.Id}' not found");
            }

            todoItem.Title = String.IsNullOrEmpty(request.Title) ? todoItem.Title : request.Title;
            todoItem.Description = String.IsNullOrEmpty(request.Description) ? todoItem.Description : request.Description;
            todoItem.IsIncomplete = request.IsIncomplete ?? todoItem.IsIncomplete;
            await _repository.UpdateTodoItemAsync(todoItem, cancellationToken);

            return todoItem;
        }
    }
}
