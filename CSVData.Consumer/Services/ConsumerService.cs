using Confluent.Kafka;
using CSVData.Consumer.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CSVData.Consumer.Services
{
    public class ConsumerService : IHostedService
    {
        private readonly string topic = "csv-data";
        private readonly string groupId = "csv-data-group";
        private readonly string bootstrapServer = "localhost:9092";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServer,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);
                        var request = JsonSerializer.Deserialize<Metric>(consumer.Message.Value);
                        Debug.WriteLine($"Device Id: {request.DeviceId}");
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
