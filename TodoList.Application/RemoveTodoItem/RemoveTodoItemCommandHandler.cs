using MediatR;
using TodoList.Domain.TodoItem;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Application.Exceptions;

namespace TodoList.Application.RemoveTodoItem
{
    public class RemoveTodoItemCommandHandler : IRequestHandler<RemoveTodoItemCommand>
    {
        private readonly ITodoItemRepository _repository;

        public RemoveTodoItemCommandHandler(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Id < 0)
            {
                throw new ArgumentException("Id cannot be less than zero.", nameof(request.Id));
            }

            var todoItem = await _repository.GetTodoItemByIdAsync(request.Id, cancellationToken);

            if (todoItem == null)
            {
                throw new NotFoundException($"Todo Item with Id '{request.Id}' not found");
            }

            await _repository.RemoveTodoItemAsync(request.Id, cancellationToken);
            return Unit.Value;
        }

    }
}