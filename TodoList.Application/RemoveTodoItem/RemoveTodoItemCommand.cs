using MediatR;

namespace TodoList.Application.RemoveTodoItem
{
    public class RemoveTodoItemCommand : IRequest
    {
        public int Id { get; }

        public RemoveTodoItemCommand(int id)
        {
            Id = id;
        }
    }
}