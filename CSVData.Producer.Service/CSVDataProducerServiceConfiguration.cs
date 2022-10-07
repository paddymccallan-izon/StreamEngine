namespace CSVData.Producer.Service
{
    using CSVData.Producer.Service.Implementations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class CSVDataProducerServiceConfiguration
    {
        public static void AddCSVDataProducerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISendMessage, SendMessage>();
        }
    }
}
