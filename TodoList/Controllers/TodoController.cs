using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TodoList.Application.ChangeTodoItem;
using TodoList.Application.CreateTodoItem;
using TodoList.Application.RemoveTodoItem;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTodoItemDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemDto createTodoItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createTodoItemDto is null)
            {
                return BadRequest(new { Message = "The request body is missing or it contains invalid JSON." });
            }

            var createTodoItemCommand = new CreateTodoItemCommand
            {
                Title = createTodoItemDto.Title,
                Description = createTodoItemDto.Description
            };

            var result = await _mediator.Send(createTodoItemCommand);

            return Created(string.Empty, result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveTodoItem(int id)
        {
            var command = new RemoveTodoItemCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ChangeTodoItemDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeTodoItem(int id, [FromBody] ChangeTodoItemDto changeTodoItemDto)
        {
            if (changeTodoItemDto is null)
            {
                return BadRequest("Todo item cannot be null");
            }

            var command = new ChangeTodoItemCommand(id, changeTodoItemDto.Title, changeTodoItemDto.Description, changeTodoItemDto.IsIncomplete);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
