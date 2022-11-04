using Newtonsoft.Json;

namespace StreamCluster5.Producer
{
    public static class InMemoryData
    {
        public static List<Metrics> GetMetrics()
        {
            return new List<Metrics>()
            {
                new Metrics()
                {
                    Date = "01/10/2022 00:00",
                    Met1 = 4,
                    Met2 = 2,
                    Met3 = 2,
                    Met4 = 6,
                    Met5 = 7,
                    Met6 = 4,
                    Met7 = 2,
                    Met8 = 5,
                    DeviceId = 1
                },
                new Metrics()
                {
                    Date = "01/10/2022 00:00",
                    Met1 = 4,
                    Met2 = 2,
                    Met3 = 2,
                    Met4 = 6,
                    Met5 = 7,
                    Met6 = 4,
                    Met7 = 2,
                    Met8 = 5,
                    DeviceId = 3
                },
            };
        }
    }

    public class Metrics
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("met_1")]
        public long Met1 { get; set; }

        [JsonProperty("met_2")]
        public long Met2 { get; set; }

        [JsonProperty("met_3")]
        public long Met3 { get; set; }

        [JsonProperty("met_4")]
        public long Met4 { get; set; }

        [JsonProperty("met_5")]
        public long Met5 { get; set; }

        [JsonProperty("met_6")]
        public long Met6 { get; set; }

        [JsonProperty("met_7")]
        public long Met7 { get; set; }

        [JsonProperty("met_8")]
        public long Met8 { get; set; }

        [JsonProperty("device_id")]
        public long DeviceId { get; set; }
    }
}