using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamEngine.Producer.Service;
using System.Text.Json;

var services = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole());

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = builder.Build();
services.AddSingleton(configuration);
services.AddStreamEngineProducerService(configuration);
var serviceProvider = services.BuildServiceProvider();
var producer = serviceProvider.GetRequiredService<ISendMessage>();

string file = Path.GetFullPath("data.txt");
var contents = File.ReadAllBytes(file);
using (MemoryStream ms = new (contents))
{
    using var reader = new StreamReader(ms);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var key = Guid.NewGuid().ToString();

        await producer.SendMessageRequest(key, line);
    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}