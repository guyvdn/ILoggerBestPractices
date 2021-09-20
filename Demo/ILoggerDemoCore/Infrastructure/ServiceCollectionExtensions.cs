using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ILoggerDemoCore.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggerDemoCore(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(ServiceCollectionExtensions));
            return serviceCollection;
        }

        public static IServiceCollection AddLoggingBehavior(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(ServiceCollectionExtensions));
            serviceCollection.TryAddTransientExact(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            serviceCollection.AddMemoryCache();
            return serviceCollection;
        }

        /// <summary>
        /// Adds a new transient registration to the service collection only when no existing registration of the same service type and implementation type exists.
        /// In contrast to TryAddTransient, which only checks the service type.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="implementationType">Implementation type</param>
        private static void TryAddTransientExact(this IServiceCollection services, Type serviceType, Type implementationType)
        {
            if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
            {
                return;
            }

            services.AddTransient(serviceType, implementationType);
        }
    }
}