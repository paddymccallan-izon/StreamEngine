using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

namespace InputCSVData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CSVController : ControllerBase
    {

        [HttpGet("GetCSVData")]
        public List<Metric> GetCsvData()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", PrepareHeaderForMatch = header => header.Header.ToLower()};
            using (var reader = new StreamReader("StreamEngine_data.csv", Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))

            {
                var records = csv.GetRecords<Metric>();
                return records.ToList();
            }
        }

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
}
