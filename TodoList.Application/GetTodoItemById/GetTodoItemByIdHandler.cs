using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TodoList.Domain.TodoItem;

namespace TodoList.Application.GetTodoItemById
{
    public class GetTodoItemByIdHandler : IRequestHandler<GetTodoByIdQuery, TodoItem>
    {
        private readonly ITodoItemRepository _repository;

        public GetTodoItemByIdHandler(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<TodoItem> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            var todoItem = await _repository.GetTodoItemByIdAsync(request.Id, cancellationToken);
            return todoItem;
        }
    }
}
