using CSVData.Producer.Service;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace InputCSVData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CSVController : ControllerBase
    {
        private readonly ISendMessage producer;

        public CSVController(ISendMessage producer)
        {
            this.producer = producer;
        }

        [HttpGet("GetCSVData")]
        public async Task<IActionResult> GetCsvData()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", PrepareHeaderForMatch = header => header.Header.ToLower() };

            DateTime startTime = DateTime.UtcNow;

            using (var reader = new StreamReader("StreamEngine_data.csv", Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Metric>();
                foreach (var record in records)
                {
                    string key = string.Empty;
                    string message = JsonSerializer.Serialize(record);
                    await producer.SendMessageRequest(key, message);
                }
            }

            DateTime endTime = DateTime.UtcNow;
            string duration = (endTime - startTime).TotalMilliseconds.ToString();

            return Ok(duration);
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
