using System.Text;
using System.Text.Json;

if (args.Length < 2 || args.Contains("-h") || args.Contains("--help"))
{
	Console.WriteLine("Usage: cl-client <host> <file_path>");
	return;
}

string host = args[0];
string filePath = args[1];
string directoryPath = Path.GetDirectoryName(filePath) ?? string.Empty;
if (directoryPath == null)
{
	Console.WriteLine("Invalid file path.");
	return;
}

var pos = new Position();

using var watcher = new FileSystemWatcher(directoryPath)
{
	Filter = Path.GetFileName(filePath),
	NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size
};

watcher.Changed += OnChanged;
watcher.EnableRaisingEvents = true;

Console.WriteLine("Press 'q' to quit the sample.");
while (Console.Read() != 'q') ;

void OnChanged(object sender, FileSystemEventArgs e)
{
	HandleFileChange(e, pos, host).Wait();
}

static async Task HandleFileChange(FileSystemEventArgs e, Position pos, string host)
{
	// Console.WriteLine(fileState.LastReadPosition);
	using FileStream fs = new(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
	fs.Seek(pos.LastReadPosition, SeekOrigin.Begin);
	using StreamReader reader = new(fs);
	string text = await reader.ReadToEndAsync();
	pos.LastReadPosition = fs.Position;
	Console.Write(text);
	using HttpClient client = new();
	var content = new StringContent(JsonSerializer.Serialize(new { text }), Encoding.UTF8, "application/json");
	HttpResponseMessage response = await client.PostAsync($"{host}/log", content);
	response.EnsureSuccessStatusCode();
}

class Position
{
	public long LastReadPosition { get; set; }
}