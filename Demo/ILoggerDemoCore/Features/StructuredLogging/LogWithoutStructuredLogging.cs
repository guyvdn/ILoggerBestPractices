using MediatR;
using Microsoft.Extensions.Logging;

namespace ILoggerDemoCore.Features.StructuredLogging
{
    public static class LogWithoutStructuredLogging
    {
        public class Request : IRequest
        {
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
                    _logger.LogInformation($"I am logging unstructured message number {i}");
                }
            }
        }
    }
}
