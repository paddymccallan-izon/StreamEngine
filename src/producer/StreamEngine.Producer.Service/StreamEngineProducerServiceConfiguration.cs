using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StreamEngine.Producer.Service
{
    public static class StreamEngineProducerServiceConfiguration
    {
        public static void AddStreamEngineProducerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISendMessage, SendMessage>();
        }
    }
}
