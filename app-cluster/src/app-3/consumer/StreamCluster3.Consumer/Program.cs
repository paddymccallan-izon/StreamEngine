using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

Console.WriteLine($"Started consumer, Ctrl-C to stop consuming");

CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => {
    e.Cancel = true;
    cts.Cancel();
};

var config = new ConsumerConfig
{
    BootstrapServers = bootstrapServer,
    GroupId = "test-consumer1",
    EnableAutoOffsetStore = false,
    EnableAutoCommit = true,
    StatisticsIntervalMs = 5000,
    SessionTimeoutMs = 6000,
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnablePartitionEof = true,
    SaslUsername = userName,
    SaslPassword = password,
    SaslMechanism = SaslMechanism.Plain,
    SecurityProtocol = SecurityProtocol.SaslSsl
};

using (var consumer = new ConsumerBuilder<Ignore, string>(config)
    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
    .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
    .SetPartitionsAssignedHandler((c, partitions) =>
    {
        Console.WriteLine(
            "Partitions incrementally assigned: [" +
            string.Join(',', partitions.Select(p => p.Partition.Value)) +
            "], all: [" +
            string.Join(',', c.Assignment.Concat(partitions).Select(p => p.Partition.Value)) +
            "]");
    })
    .SetPartitionsRevokedHandler((c, partitions) =>
    {
        var remaining = c.Assignment.Where(atp => partitions.Where(rtp => rtp.TopicPartition == atp).Count() == 0);
        Console.WriteLine(
            "Partitions incrementally revoked: [" +
            string.Join(',', partitions.Select(p => p.Partition.Value)) +
            "], remaining: [" +
            string.Join(',', remaining.Select(p => p.Partition.Value)) +
            "]");
    })
    .SetPartitionsLostHandler((c, partitions) =>
    {
        Console.WriteLine($"Partitions were lost: [{string.Join(", ", partitions)}]");
    })
    .Build())
{
    consumer.Subscribe(topic);

    try
    {
        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume(cts.Token);

                if (consumeResult.IsPartitionEOF)
                {
                    Console.WriteLine(
                        $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                    continue;
                }

                Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                try
                {
                    consumer.StoreOffset(consumeResult);
                }
                catch (KafkaException e)
                {
                    Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Consume error: {e.Error.Reason}");
            }
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Closing consumer.");
        consumer.Close();
    }
}

