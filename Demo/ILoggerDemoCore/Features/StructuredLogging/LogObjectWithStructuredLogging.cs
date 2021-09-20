using MediatR;
using Microsoft.Extensions.Logging;

namespace ILoggerDemoCore.Features.StructuredLogging
{
    public static class LogObjectWithStructuredLogging
    {
        public class Message
        {
            public string GreetingType { get; } 
            public string GreetingTime { get; }
            public string GreetingTo { get; }

            public Message(string greetingType, string greetingTime, string greetingTo)
            {
                GreetingType = greetingType;
                GreetingTime = greetingTime;
                GreetingTo = greetingTo;
            }

            public override string ToString()
            {
                return $"{GreetingType} {GreetingTime} {GreetingTo}";
            }
        }
        
        public class Request : IRequest
        {
            public bool Objectify { get; set; }
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
                var message = new Message("Good", "Morning", "James Bond");

                if (request.Objectify)
                {
                    _logger.LogInformation("I am logging structured message: {@Message}", message);
                }
                else
                {
                    _logger.LogInformation("I am logging structured message: {Message}", message);
                }
            }
        }
    }
}