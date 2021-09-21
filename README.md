---
title: ILogger Demo
---

## Loggen met ILogger in .NET

<em>Aanbevelingen en best practices</em>
<br/><br/>
Guy Van den Nieuwenhof
<br/>
<a href="https://github.com/guyvdn">github.com/guyvdn</a>
---

### Agenda

<ul>
  <li class="fragment fade-in-then-semi-out"><a href="#">ILogger</a> vs <a href="#">ILoggerProvider</a> vs <a href="#">ILoggerFactory</a></li>
  <li class="fragment fade-in-then-semi-out">Structured logging</li>
  <li class="fragment fade-in-then-semi-out">Scopes</li>
	<li class="fragment fade-in-then-semi-out">Log levels</li>
	<li class="fragment fade-in-then-semi-out">Serilog en Seq</li>
  <li class="fragment fade-in-then-semi-out">LoggingBehavior</li>
</ul>

---
 
### ILogger
<em>vs</em>
### ILoggerProvider
<em>vs</em>
### ILoggerFactory

----

### ILogger

<ul>
  <li class="fragment fade-in-then-semi-out">Is verantwoordelijk voor het wegschrijven van berichten met een bepaald <a href="#">log level</a> en <a href="#">logging scope</a></li>
  <li class="fragment fade-in-then-semi-out">De interface bevat enkele generic methods die gebruikt worden door extension methods zoals <a href="#">LogInformation</a> of <a href="#">LogError</a></li>
  <li class="fragment fade-in-then-semi-out">Kan typed zijn (<a href="#">ILogger&lt;T&gt;</a>) of untyped (<a href="#">ILogger</a>)</li>
</ul>

----

### ILoggerProvider

<ul>
    <li class="fragment fade-in-then-semi-out">Een logger provider is een logging sink implementie, bvb. console, app insights of file</li>
    <li class="fragment fade-in-then-semi-out">De enige verantwoordelijkheid is om <a href="#">ILogger</a> instanties aan te maken die naar een sink loggen</li>
    <li class="fragment fade-in-then-semi-out">Een logger instantie die aangemaakt is door een provider zal enkel loggen naar deze provider</li>
</ul>

----

### ILoggerFactory

<ul>
  <li class="fragment fade-in-then-semi-out">De <a href="#">ILoggerFactory</a> is de bootstrapper van het logging systeem</li>
  <li class="fragment fade-in-then-semi-out">Wordt gebruikt om logger providers en logger instanties te verbinden</li>
  <li class="fragment fade-in-then-semi-out">De logger instanties zullen loggen naar alle geregistreerde logger providers</li>
</ul>

----

### Besluit

<ul>
  <li class="fragment fade-in-then-semi-out">Gebruik <a href="#">ILogger&lt;T&gt;</a> in combinatie met Dependency Injection</li>
  <li class="fragment fade-in-then-semi-out">Laat het aanmaken van <a href="#">ILoggerProvider</a> en <a href="#">ILoggerFactory</a> instanties over aan middleware zoals Serilog of NLog</li>
</ul>

---

## Structured logging

----

### Gebruik structured logs

<ul>
  <li class="fragment fade-in-then-semi-out">Gebruik steeds structured logs</li>
  <li class="fragment fade-in-then-semi-out">De backend ontvangt hierdoor de placeholders en hun waarden afzonderlijk</li>
  <li class="fragment fade-in-then-semi-out">Elk log statement bewaart zo alle properties en een template hash (in AI “MessageTemplate”)</li>
  <li class="fragment fade-in-then-semi-out">Op deze manier kan je geavanceerd filteren of alle logs van een bepaald type opvragen</li>
  <li class="fragment fade-in-then-semi-out">Serialisatie van properties gebeurd pas als de log effectief geschreven wordt (in tegenstelling tot string interpolation)</li>
</ul>

----

### Gebruik structured logs

```csharp
logger.LogWarning("The person {PersonId} could not be found.", personId);
```
<!-- .element class="fragment fade-in-then-semi-out" -->

Dit statement zal volgende properties loggen
<!-- .element class="fragment fade-in-then-semi-out" -->

<ul>
  <li class="fragment fade-in-then-semi-out">Message: The person 5 could not be found</li>
  <li class="fragment fade-in-then-semi-out">MessageTemplate: The person {PersonId} could not be found</li>
  <li class="fragment fade-in-then-semi-out">PersonId: 5</li>
</ul>

----

### Gebruik structured logs

<em>Notes</em>

<ul>
  <li class="fragment fade-in-then-semi-out">Gebruik <u>geen punten</u> in de placeholders, sommige <a href="#">Ilogger</a> implementaties ondersteunen dit niet</li> 
  <li class="fragment fade-in-then-semi-out">Gebruik <a href="#">@</a> in de placeholders om de structuur van objecten te behouden</li>
</ul>

----

### Demo

---

## Log levels

----

### Gebruik het correcte log level

<ul>
  <li class="fragment fade-in-then-semi-out">Om aanmaken van alerts en tickets te automatiseren</li>
  <li class="fragment fade-in-then-semi-out">Om geen gevoelige gegevens te exposen</li>
  <li class="fragment fade-in-then-semi-out">Om geen onnodige kosten te genereren</li>
</ul>

----

### Trace/Verbose 

<ul>
  <li class="fragment fade-in-then-semi-out">De meest gedetailleerde logs</li>
  <li class="fragment fade-in-then-semi-out">Deze mogen gevoelige applicatie data bevatten</li>
  <li class="fragment fade-in-then-semi-out">Ze zijn standaard gedisabled en mogen nooit in een productie omgeving aan gezet worden</li>
</ul>

----

### Debug

<ul>
  <li class="fragment fade-in-then-semi-out">Worden gebruikt tijdens development</li>
  <li class="fragment fade-in-then-semi-out">Bevatten informatie die nuttig is om te debuggen en bevatten geen lange termijn waarde</li>
</ul>

----

### Information

<ul>
  <li class="fragment fade-in-then-semi-out">Om de flow van de applicatie te volgen</li>
  <li class="fragment fade-in-then-semi-out">Zouden ook geen lange termijn waarde mogen bevatten</li>
</ul>

----

### Warning

<ul>
  <li class="fragment fade-in-then-semi-out">Om abnormaal of onverwacht gedrag in de applicatie weer te geven</li>
  <li class="fragment fade-in-then-semi-out">Dit gedrag zorgt er echter niet voor dat de applicatie stopt</li>
</ul>

----

### Error

<ul>
  <li class="fragment fade-in-then-semi-out">Om aan te duiden dat de normale flow van de applicatie gestopt is omwille van een fout</li>
  <li class="fragment fade-in-then-semi-out">Deze duiden een fout aan in de huidige activiteit, geen storing voor de hele applicatie</li>
</ul>

----

### Critical

<ul>
  <li class="fragment fade-in-then-semi-out">Om een onherstelbare fout of systeemcrash te beschrijven</li>
  <li class="fragment fade-in-then-semi-out">Onmiddellijke aandacht is vereist om de toepassing terug normaal te laten werken</li>
</ul>

---

## Exception logging

----

### Exception logging

<ul>
  <li class="fragment fade-in-then-semi-out">Geef de exception altijd door als het eerste argument. Anders wordt de exception als custom property behandeld</li>
  <li class="fragment fade-in-then-semi-out">Als er geen placeholder in het bericht aanwezig is zal de exception dan ook niet gelogged worden</li>
  <li class="fragment fade-in-then-semi-out">Er zijn tevens exception specifieke overloads om de errors te formatteren en op te slaan</li>
</ul>

```csharp
logger.LogWarning(exception, "An exception occured")
```
<!-- .element: class="fragment" -->

----

### Demo

---

## Scopes

----

### Scopes


<ul>
    <li class="fragment fade-in-then-semi-out">Gebruik scopes om custom properties toe te voegen aan alle logs binnen een specifieke context</li>
</ul>

```csharp [1|2|4,5]
using (logger.BeginScope(
    new Dictionary<string, object> { {"PersonId", 5 } }))
{
    logger.LogInformation("Hello");
    logger.LogInformation("World");
}
```

<!-- .element: class="fragment" -->
<ul>
  <li class="fragment fade-in-then-semi-out">Beide logs zullen nu een extra PersonId property bevatten</li>
  <li class="fragment fade-in-then-semi-out">De meeste scope implementaties werken zelfs correct in parallele code omdat ze de async context gebruiken</li>
</ul>

---

## Serilog en Seq

----

### Serilog - appsettings.json

```json [6|7-9|10-16|18|19-22]
{
  "AllowedHosts": "*",
  "ApplicationInsights": {
    "InstrumentationKey": "UserSecret"
  },
  "Serilog": {
    "Using": [
      "Serilog.Sinks.ApplicationInsights"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    // Install Serilog.Enrichers.(Environment, Thread and Process)
    "Enrich": [ "FromLogContext", "WithMachineName", "WithProcessId", "WithThreadId" ],
    "Properties": {
      "Application": "ILoggerDemo",
      "Environment": "Development"
    },
    ✂
```

<!-- .element: class="fragment" -->

----

```json [2|3-5|6-12|13-19|20-28|29-34]
    ✂
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "Path": "C:\\Temp\\Logs\\logSerilog.txt",
          "outputTemplate": "{Timestamp:G} {Message}{NewLine:1}{Exception:1}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "Path": "C:\\Temp\\Logs\\logSerilog.json",
          "formatter": "Serilog.Formatting.Json.JsonFormatter, Serilog"
        }
      },
      {
        "Name": "ApplicationInsights",
        "Args": {
          "restrictedToMinimumLevel": "Information",
          "telemetryConverter": "Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.
          TelemetryConverters.TraceTelemetryConverter, Serilog.Sinks.ApplicationInsights",
          "instrumentationKey": "UserSecret"
        }
      },
      { 
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://localhost:8081"
        }
      }
    ]
  }
}
```

----

### Serilog - Program.cs

```csharp [3,9-11|3,15|3,20|3,24|28,32]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting Up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, @"The application failed to start correctly ¯\_(ツ)_/¯");
            }
            finally
            {
                Log.CloseAndFlush(); // close the log correctly
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
```
----

### SEQ 

Lokaal gebruiken

```ps
docker run -d --restart unless-stopped --name seq -e ACCEPT_EULA=Y -v C:\Temp\Logs:/data -p 8081:80 datalust/seq:latest
```

---

## LoggingBehavior

----

### LoggingBehavior

<ul>
  <li class="fragment fade-in-then-semi-out"><a href="#">LoggingBehavior</a> voor <a href="#">MediatR pipeline</a></li>
  <li class="fragment fade-in-then-semi-out">Het behavior zal de start van de <a href="#">Request</a> loggen alsook het einde met duration</li>
  <li class="fragment fade-in-then-semi-out"><a href="#">Exceptions</a> worden ook gelogged en terug doorgesmeten</li>
  <li class="fragment fade-in-then-semi-out">Het behavior maakt een <a href="#">LoggingScope</a> aan die ook alle logs omvat die binnen de handler worden uitgevoerd</li>
</ul>

----

### Registratie van LoggingBehavior

Via ServiceCollection extension

<!-- .element: class="fragment" -->

```csharp [3|4|5]
public static IServiceCollection AddLoggingBehavior(this IServiceCollection serviceCollection)
{
    serviceCollection.AddMediatR(typeof(ServiceCollectionExtensions));
    serviceCollection.AddMemoryCache();
    serviceCollection.TryAddTransientExact(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));    
    return serviceCollection;
}
```

<!-- .element: class="fragment" -->

----

### TryAddTransientExact

Deze doet controle op <a href="#">ServiceType</a> én <a href="#">ImplementationType</a>

<!-- .element: class="fragment" -->

```csharp [3|8]
private static void TryAddTransientExact(this IServiceCollection services, Type serviceType, Type implementationType)
{
    if (services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType))
    {
        return;
    }

    services.AddTransient(serviceType, implementationType);
}
```

<!-- .element: class="fragment" -->

----

### IncludeInLogsAttribute

Dit Attribute kan je gebruiken om extra <a href="#">properties</a> van de <a href="#">MediatR request</a> te loggen in de <a href="#">LogginScope</a>
<!-- .element: class="fragment" -->

```csharp [3,4]
public class Request : IRequest<Response>
{
    [IncludeInLogs]
    public Guid LoggedProperty { get; set; }

    public Guid NotLoggedProperty { get; set; }
}
```
<!-- .element: class="fragment" -->

Het is een opt-in verhaal. `LoggedProperty` zal dus gelogged worden en `NotLoggedProperty` niet.
<!-- .element: class="fragment" -->

----

### Demo

---

### Sources

<div style="font-size:0.6em">
<ul>
  <li class="fragment fade-in-then-semi-out">Rico Suter - Logging with ILogger in .NET: Recommendations and best practices
  <br/><em>https://blog.rsuter.com/logging-with-ilogger-recommendations-and-best-practices</em></li>
  <li class="fragment fade-in-then-semi-out">Tim Corey - Logging in .NET Core 3.0 and Beyond - Configuration, Setup, and More
  <br/><em>https://www.youtube.com/watch?v=oXNslgIXIbQ</em></li>
  <li class="fragment fade-in-then-semi-out">Tim Corey - C# Logging with Serilog and Seq - Structured Logging Made Easy 
  <br/><em>https://www.youtube.com/watch?v=_iryZxv8Rxw</em></li>
  <li class="fragment fade-in-then-semi-out">Serilog - Structured Data
  <br/><em>https://github.com/serilog/serilog/wiki/Structured-Data</em></li>
</ul>

---

## Vragen?

</div>