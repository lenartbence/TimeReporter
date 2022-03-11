using System;

namespace TimeReporter.Model
{
    public class Day
    {
        public DateTime Date { get; set; }

        public string Project { get; set; }

        public DayType Type { get; set; }
    }
}
