using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;

namespace API
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.File(new RenderedCompactJsonFormatter(), "/logs/DomainSetuplog.ndjson",
                   rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true, retainedFileCountLimit: 3)
                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                   .MinimumLevel.Override("System", LogEventLevel.Warning)
                   .CreateLogger();

            try
            {
                Log.Information($"Starting up  {DateTime.UtcNow}!");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error($"Something went wrong. UTC: {DateTime.UtcNow}");
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        [Obsolete]
        public static IHostBuilder CreateHostBuilder(string[] args) =>
                  Host.CreateDefaultBuilder(args)
#if (DEBUG)
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddDebug();
                logging.AddConsole();
                logging.AddEventSourceLogger();
                logging.AddSerilog();
            })
#endif
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // Set properties and call methods on options
                })
                .UseSerilog()
                .UseIISIntegration()
                .UseStartup<Startup>();
            })
                  .ConfigureAppConfiguration(appBuilder =>
                  {
                      appBuilder.AddEnvironmentVariables();
                  });
    }
}