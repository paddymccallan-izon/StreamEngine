using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

var services = new ServiceCollection()
    .AddLogging(logging => logging.AddConsole());

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = builder.Build();
services.AddSingleton(configuration);
var serviceProvider = services.BuildServiceProvider();

var bootstrapServer = configuration.GetSection("BOOTSTRAP_SERVER").Value;
var userName = configuration.GetSection("CREDENTIALS").GetSection("USER_NAME").Value;
var password = configuration.GetSection("CREDENTIALS").GetSection("PASSWORD").Value;
var topic = configuration.GetSection("TOPICS").GetSection("TEST_TOPIC").Value;

var config = new ProducerConfig
{
    Acks = Acks.All,
    BootstrapServers = bootstrapServer,
    SecurityProtocol = SecurityProtocol.SaslSsl,
    SaslMechanism = SaslMechanism.Plain,
    SaslUsername = userName,
    SaslPassword = password,
    ClientId = Dns.GetHostName(),
};

Action<DeliveryReport<string, string>> handler = r =>
    Console.WriteLine(!r.Error.IsError
        ? $"Delivered message to {r.TopicPartitionOffset} "
        : $"Delivery error: {r.Error.Reason} ");

string file = Path.GetFullPath("data.txt");
var contents = File.ReadAllBytes(file);
using (MemoryStream ms = new(contents))
{
    using var reader = new StreamReader(ms);

    using var producer = new ProducerBuilder<string, string>(config).Build();

    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        var key = Guid.NewGuid().ToString();
        if (!string.IsNullOrEmpty(line))
        {
            producer.Produce(topic, new Message<string, string> { Key = key, Value = line.ToString() }, handler);
            Console.WriteLine(DateTime.UtcNow.ToString());
            _ = int.TryParse(configuration.GetSection("THREAD_SLEEP").Value, out int sleepTime);
            Thread.Sleep(sleepTime);
        }
    }
}