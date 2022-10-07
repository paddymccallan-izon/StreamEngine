namespace CSVData.Producer.Service.Implementations
{
    using Confluent.Kafka;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class SendMessage : ISendMessage
    {
        private readonly string topic;
        private readonly string bootstrapServer;
        private readonly IConfiguration configuration;
        private readonly ILogger<SendMessage> logger;

        public SendMessage(
            IConfiguration configuration
            , ILogger<SendMessage> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            bootstrapServer = this.configuration.GetSection("BOOTSTRAP_SERVER").Value;
            topic = this.configuration.GetSection("TOPICS").GetSection("CSV_TOPIC").Value;
        }
        public async Task<bool> SendMessageRequest(string key, string message)
        {
            try
            {
                var config = new ProducerConfig
                {
                    Acks = Acks.All,
                    BootstrapServers = this.bootstrapServer,
                    ClientId = Dns.GetHostName(),
                };

                using var producer = new ProducerBuilder<string, string>(config).Build();

                try
                {
                    producer.Produce(topic, new Message<string, string>
                    {
                        Key = key,
                        Value = message
                    }, (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError)
                        {
                            logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else
                        {
                            logger.LogInformation(message: $"Produced message to: {deliveryReport.TopicPartitionOffset} " +
                                $"\n Delivery timestamp: {deliveryReport.Timestamp}");
                        }
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("", ex);
                }
                finally
                {
                    var queueSize = producer.Flush(TimeSpan.FromSeconds(5));
                    if (queueSize > 0)
                    {
                        logger.LogWarning("WARNING: Producer event queue has " + queueSize + " pending events on exit.");
                    }
                    producer.Dispose();
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(true);
        }
    }
}
