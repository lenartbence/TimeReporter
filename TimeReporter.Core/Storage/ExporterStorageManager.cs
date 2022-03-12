using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TimeReporter.Model;

namespace TimeReporter.Core.Storage
{
    internal class ExporterStorageManager : IStorageManager<ExporterDto>
    {
        private string _jsonPath = "storage/exporters.json";

        public IEnumerable<ExporterDto> Load()
        {
            if (!File.Exists(_jsonPath))
            {
                return new List<ExporterDto>();
            }

            string json = File.ReadAllText(_jsonPath);
            return JsonConvert.DeserializeObject<IEnumerable<ExporterDto>>(json);
        }

        public void Save(IEnumerable<ExporterDto> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(_jsonPath, json);
        }
    }
}
