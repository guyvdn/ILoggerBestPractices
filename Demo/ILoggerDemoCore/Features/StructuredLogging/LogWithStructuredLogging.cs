using ILoggerDemoCore.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ILoggerDemoCore.Features.StructuredLogging
{
    public static class LogWithStructuredLogging
    {
        public class Request : IRequest
        {
            [IncludeInLogs]
            public int NumberOfMessagesToLog { get; set; }
        }

        internal class Handler : RequestHandler<Request>
        {
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger;
            }

            protected override void Handle(Request request)
            {
                for (var i = 0; i < request.NumberOfMessagesToLog; i++)
                {
                    _logger.LogInformation("I am logging structured message number {MessageNumber}", i);
                }
            }
        }
    }
}