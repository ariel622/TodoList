using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Application.CreateTodoItem
{
    public class CreateTodoItemCommand : IRequest<CreateTodoItemDto>
    {
        public string Title { get; set; }
        
        public string? Description { get; set; }
    }
}
