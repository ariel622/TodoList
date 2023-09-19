using MediatR;
using TodoList.Domain.TodoItem;

namespace TodoList.Application.GetTodoItemById
{
    public class GetTodoByIdQuery : IRequest<TodoItem>
    {
        public int Id { get; set; }

        public GetTodoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
