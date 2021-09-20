using System;

namespace ILoggerDemoCore.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IncludeInLogsAttribute : Attribute { }
}