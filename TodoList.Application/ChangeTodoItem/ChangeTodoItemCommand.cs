using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Domain.TodoItem;

namespace TodoList.Application.ChangeTodoItem
{
    public class ChangeTodoItemCommand : IRequest<TodoItem>
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsIncomplete { get; set; }

        public ChangeTodoItemCommand(int id, string? title, string? description, bool? isIncomplete)
        {
            Id = id;
            Title = title;
            Description = description;
            IsIncomplete = isIncomplete;    
        }
    }
}
