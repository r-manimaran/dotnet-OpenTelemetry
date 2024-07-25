using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace opentel;

public static class RegistrationExtensions
{
    public static void RegisterOpenTelemetry(this WebApplicationBuilder builder)
    {
        // Added code to configure OpenTelemetry logging
        builder.Logging.ClearProviders();
        //builder.Logging.AddOpenTelemetry(opt=> opt.AddConsoleExporter());
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;

            options.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                .AddService("WeatherForecast")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["environment"] = builder.Environment.EnvironmentName
                })
            );

            options.AddOtlpExporter(x =>
            {
                x.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
                x.Protocol = OtlpExportProtocol.HttpProtobuf;
                x.Headers = "X-Seq-ApiKey=VySRbfZx4pUMZ67coP6g";
            });
        });
    }
}