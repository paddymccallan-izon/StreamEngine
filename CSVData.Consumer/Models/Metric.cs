using Newtonsoft.Json;

namespace CSVData.Consumer.Models
{
    public class Metric
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("_met1")]
        public int? Met1 { get; set; }

        [JsonProperty("_met2")]
        public int? Met2 { get; set; }

        [JsonProperty("_met3")]
        public int? Met3 { get; set; }

        [JsonProperty("_met4")]
        public int? Met4 { get; set; }

        [JsonProperty("_met5")]
        public int? Met5 { get; set; }

        [JsonProperty("_met6")]
        public int? Met6 { get; set; }

        [JsonProperty("_met7")]
        public int? Met7 { get; set; }

        [JsonProperty("_met8")]
        public int? Met8 { get; set; }

        [JsonProperty("device_id")]
        public int? DeviceId { get; set; }
    }
}
