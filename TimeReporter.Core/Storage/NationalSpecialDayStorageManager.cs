using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Storage
{
    internal class NationalSpecialDayStorageManager : IStorageReader<Day, DateTime>
    {
        private Dictionary<string, DayType> dayTypeMap = new Dictionary<string, DayType>()
        {
            ["Munkaszüneti nap"] =  DayType.NationalHoliday,
            ["Pihenőnap"] = DayType.NationalHoliday,
            ["Áthelyezett munkanap"] = DayType.Work
        };

        public IEnumerable<Day> Load(DateTime parameter)
        {
            string path = Path.Join("storage/specialdays", $"{parameter.Year}.txt");

            try
            {
                var result = new List<Day>();

                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    string[] columns = line.Split("\t");
                    DateTime date = DateTime.ParseExact(columns.First(), "dd.MM", null);
                    DayType dayType = dayTypeMap[columns.Last()];

                    result.Add(new Day()
                    {
                        Date = date,
                        Type = dayType
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log
                return null;
            }
        }
    }
}
