using Serilog;
using Serilog.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoList.Application.ChangeTodoItem;
using TodoList.Domain.TodoItem;
using TodoList.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Message = e.Value.Errors.First().ErrorMessage
            })
            .ToArray();

        return new BadRequestObjectResult(errors);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddMediatR(typeof(ChangeTodoItemCommand).Assembly);
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingDecorator<,>));


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7))
    .CreateLogger();


builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
