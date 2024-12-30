var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "default port";
Console.WriteLine($"Server started on {urls}");

app.MapGet("/", () => "Hello World!");

app.MapPost("/log", (LogMessage logMessage) =>
{
	Console.Write(logMessage.Text);
	return logMessage.Text;
});

app.Run();


public class LogMessage
{
	public string? Text { get; set; }
}