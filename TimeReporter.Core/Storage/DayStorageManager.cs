using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Storage
{
    internal class DayStorageManager : IStorageManager<Day, DateTime>
    {
        private string _directoryPath = "storage/days/";

        public IEnumerable<Day> Load(DateTime parameter)
        {
            string path = GetFilePath(parameter);

            if (!File.Exists(path))
            {
                return new List<Day>();
            }

            var json = File.ReadAllText(path);
            var dtos = JsonConvert.DeserializeObject<List<Day>>(json);
            return dtos;
        }

        public void Save(IEnumerable<Day> days)
        {
            if (days == null || !days.Any())
                return;

            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }

            var json = JsonConvert.SerializeObject(days, Formatting.Indented);
            File.WriteAllText(GetFilePath(days.First().Date), json);
        }

        private string GetFilePath(DateTime date) => Path.Join(_directoryPath, $"{date:yyyy-MM}.json");
    }
}
