var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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