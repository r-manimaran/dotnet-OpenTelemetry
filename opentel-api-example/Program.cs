using opentel;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog.Sinks.OpenTelemetry;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Used Serilog logging along with OpenTelemtry and Seq
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(options =>
    {
        options.ResourceAttributes = new Dictionary<string, object>
        {
            { "service.name", "weatherapi" },
            { "service.instance.id", Environment.MachineName }
        };
        options.Endpoint = "http://localhost:5341/ingest/otlp/v1/logs";
        options.Protocol = OtlpProtocol.HttpProtobuf;
        options.Headers = new Dictionary<string, string>
        {
            ["X-Seq-ApiKey"]= "VySRbfZx4pUMZ67coP6g"
        };
    })
    .CreateLogger();

builder.Services.AddSerilog();

//Register the OpenTelemetry from Extesion Helper method
//Used Default Logging, OpenTelemetry and logged to Seq
//builder.RegisterOpenTelemetry();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (string city, int days, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting next {days} weather forecast for {City} ", days, city);
    var forecast = Enumerable.Range(1, days).Select(index =>
        new WeatherForecast
        (
            city,
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    logger.LogInformation("Returning {WeatherCount} weather forecast for the City {city}", forecast.Length, city);
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(string city, DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
