using CSVData.Producer;
using CSVData.Producer.Service;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.Json;

var services = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole());

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

IConfiguration configuartion = builder.Build();
services.AddSingleton(configuartion);

services.AddCSVDataProducerService(configuartion);

var serviceProvider = services.BuildServiceProvider();
var producer = serviceProvider.GetRequiredService<ISendMessage>();

var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", PrepareHeaderForMatch = header => header.Header.ToLower() };

DateTime startTime = DateTime.UtcNow;

using (var reader = new StreamReader("StreamEngine_data.csv", Encoding.UTF8))
using (var csv = new CsvReader(reader, config))
{
    var records = csv.GetRecords<Metric>();
    foreach (var record in records)
    {
        string key = string.Empty;
        string message = JsonSerializer.Serialize(record);
        await producer.SendMessageRequest(key, message);
    }
}

DateTime endTime = DateTime.UtcNow;
TimeSpan duration = endTime - startTime;
var totalTime = duration.TotalMilliseconds;

Console.WriteLine("Total time taken: " + totalTime.ToString());

