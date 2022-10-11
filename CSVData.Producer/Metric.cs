using CsvHelper.Configuration.Attributes;

namespace CSVData.Producer
{
    public class Metric
    {
        [Index(0)]
        public string? date { get; set; }
        [Index(1)]
        public int? _met1 { get; set; }
        [Index(2)]
        public int? _met2 { get; set; }
        [Index(3)]
        public int? _met3 { get; set; }
        [Index(4)]
        public int? _met4 { get; set; }
        [Index(5)]
        public int? _met5 { get; set; }
        [Index(6)]
        public int? _met6 { get; set; }
        [Index(7)]
        public int? _met7 { get; set; }
        [Index(8)]
        public int? _met8 { get; set; }
        [Index(9)]
        public int? device_id { get; set; }
    }
}
