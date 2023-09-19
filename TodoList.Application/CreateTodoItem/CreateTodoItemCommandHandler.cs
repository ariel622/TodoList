using MediatR;
using System.ComponentModel.DataAnnotations;
using TodoList.Application.Sanitization;
using TodoList.Domain.TodoItem;

namespace TodoList.Application.CreateTodoItem
{
    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, CreateTodoItemDto>
    {
        private readonly ITodoItemRepository _repository;

        public CreateTodoItemCommandHandler(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<CreateTodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            request.Title = InputSanitizer.SanitizeInput(request.Title);
            request.Description = InputSanitizer.SanitizeInput(request.Description);

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ValidationException("Title cannot be null or whitespace.");
            }

            var todoItem = new TodoItem
            {
                Title = request.Title,
                Description = request.Description,
                IsIncomplete = true,
                CreatedDate = DateTime.Now
            };

            await _repository.CreateTodoItemAsync(todoItem, cancellationToken);

            return new CreateTodoItemDto { Description = request.Description, Title = request.Title };
        }
    }
}
