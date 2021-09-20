using ILoggerDemoCore;
using ILoggerDemoCore.Features.ExceptionLogging;
using ILoggerDemoCore.Features.StructuredLogging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ILoggerDemoService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoggerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("LogWithoutStructuredLogging")]
        public void LogWithoutStructuredLogging(int numberOfMessagesToLog = 10)
        {
            _mediator.Send(new LogWithoutStructuredLogging.Request { NumberOfMessagesToLog = numberOfMessagesToLog });
        }

        [HttpPost]
        [Route("LogWithtructuredLogging")]
        public void LogWithtructuredLogging(int numberOfMessagesToLog = 10)
        {
            _mediator.Send(new LogWithStructuredLogging.Request { NumberOfMessagesToLog = numberOfMessagesToLog });
        }    
        
        [HttpPost]
        [Route("LogObjectWithtructuredLogging")]
        public void LogObjectWithtructuredLogging()
        {
            _mediator.Send(new LogObjectWithStructuredLogging.Request {  Objectify = false});
        }  
        
        [HttpPost]
        [Route("LogObjectWithtructuredLoggingObjectified")]
        public void LogObjectWithtructuredLoggingObjectified()
        {
            _mediator.Send(new LogObjectWithStructuredLogging.Request {  Objectify = true});
        }

        [HttpPost]
        [Route("LogExceptionIncorrectly")]
        public void LogExceptionIncorrectly()
        {
            _mediator.Send(new LogExceptionIncorrectly.Request());
        }

        [HttpPost]
        [Route("LogExceptionCorrectly")]
        public void LogExceptionCorrectly()
        {
            _mediator.Send(new LogExceptionCorrectly.Request());
        }
    }
}
