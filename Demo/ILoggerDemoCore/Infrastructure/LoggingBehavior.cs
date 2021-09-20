using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ILoggerDemoCore.Infrastructure
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        public LoggingBehavior(IMemoryCache memoryCache, ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestType = request.GetType().FullName;
            var state = GetRequestState(request);
            var stopwatch = Stopwatch.StartNew();

            using (_logger.BeginScope(state))
            {
                try
                {
                    _logger.LogInformation("Execution of {MediatrRequestType} started", requestType);

                    var response = await next();

                    stopwatch.Stop();
                    _logger.LogInformation("Execution of {MediatrRequestType} finished in {MediatrRequestDuration}ms", requestType, stopwatch.ElapsedMilliseconds);

                    return response;
                }
                catch (Exception e)
                {
                    stopwatch.Stop();
                    _logger.LogError(e, "Execution of {MediatrRequestType} failed after {MediatrRequestDuration}ms", requestType, stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        private Dictionary<string, object> GetRequestState(TRequest request)
        {
            var requestType = request.GetType();
            var requestState = new Dictionary<string, object> { { "MediatrRequestType", requestType.FullName } };
            var requestProperties = GetRequestProperties(requestType);

            foreach (var property in requestProperties)
            {
                var propValue = property.GetValue(request, null);
                requestState.Add(property.Name, propValue);
            }

            return requestState;
        }

        private IEnumerable<PropertyInfo> GetRequestProperties(Type requestType)
        {
            var cacheKey = $"LoggingBehavior_{requestType.FullName}";

            if (_memoryCache.TryGetValue(cacheKey, out var cachedValue))
            {
                return (IEnumerable<PropertyInfo>)cachedValue;
            }

            var requestProperties = requestType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && Attribute.IsDefined(p, typeof(IncludeInLogsAttribute)));

            return _memoryCache.Set(cacheKey, requestProperties);
        }
    }
}