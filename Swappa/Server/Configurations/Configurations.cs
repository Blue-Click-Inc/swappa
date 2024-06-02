using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Swappa.Server.Configurations
{
    public static class Configurations
    {
        public static void ConfigureLogging()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(config, env))
                .Enrich.WithProperty("Environment", env)
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }

        public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot config, string environment)
        {
            var conf = new ElasticsearchSinkOptions(new Uri(config["ElasticConfig:Uri"] ?? string.Empty))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName()?.Name?.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
            return conf;
        }
    }
}
