using Newtonsoft.Json;

namespace StreamEngine.Producer
{
    public static class InMemoryData
    {
        public static List<Metric> LoadJson()
        {
            using StreamReader r = new StreamReader("data.json");
#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<List<Metric>>(r.ReadToEnd());
#pragma warning restore CS8603 // Possible null reference return.
        } 
    }

    public class Metric
    {
        public string? date { get; set; }
        public int? _met1 { get; set; }
        public int? _met2 { get; set; }
        public int? _met3 { get; set; }
        public int? _met4 { get; set; }
        public int? _met5 { get; set; }
        public int? _met6 { get; set; }
        public int? _met7 { get; set; }
        public int? _met8 { get; set; }
        public int? device_id { get; set; }
    }
}
