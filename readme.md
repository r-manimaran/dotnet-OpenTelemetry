# Setting up OpenTelementry logging with .Net, Serilog and Seq


 ```bash
# Create the new dotnet web api project
> dotnet new webapi -n opentel-api-example

#Add the nuget pacakges - ConsoleExporter
> dotnet add package OpenTelemetry.Exporter.Console --version 1.9.0

> dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol --version 1.9.0

> dotnet add package OpenTelemetry.Instrumentation.AspNetCore --version 1.9.0

```


## Push Logs to Seq
![alt text](image.png)