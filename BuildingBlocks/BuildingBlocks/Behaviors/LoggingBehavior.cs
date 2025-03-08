using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request of type {requestType}. Payload: {requestData}",
            typeof(TRequest).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var elapsedTime = timer.Elapsed;
        if (elapsedTime.Seconds > 3) // Log warnings if the request handling took more than 3 seconds
            logger.LogWarning("Request of type {requestType} took {elapsedTime:F2} seconds to process",
                typeof(TRequest).Name, elapsedTime);

        logger.LogInformation("Successfully handled {requestType}. Response Type: {responseType}",
            typeof(TRequest).Name, typeof(TResponse).Name);
        return response;
    }
}
