using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

public class LoggingDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;

    public LoggingDecorator(ILogger<LoggingDecorator<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling command {CommandName} ({@Command})", typeof(TRequest).Name, request);

        var response = await next();

        _logger.LogInformation("Command {CommandName} handled - response: {@Response}", typeof(TRequest).Name, response);

        return response;
    }
}
