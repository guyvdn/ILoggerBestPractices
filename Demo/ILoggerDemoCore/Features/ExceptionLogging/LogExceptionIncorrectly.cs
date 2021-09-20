using System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ILoggerDemoCore.Features.ExceptionLogging
{
    public static class LogExceptionIncorrectly
    {
        public class Request : IRequest { }
        internal class Handler : RequestHandler<Request>
        {
            private readonly ILogger<Handler> _logger;

            public Handler(ILogger<Handler> logger)
            {
                _logger = logger;
            }

            protected override void Handle(Request request)
            {
                try
                {
                    throw new InvalidOperationException("You are not allowed to do this");
                }
                catch (Exception e)
                {
                    _logger.LogError("Error in Handler", e);
                    throw;
                }
            }
        }
    }
}