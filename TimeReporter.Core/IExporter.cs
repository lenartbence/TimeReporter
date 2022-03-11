using System.Collections.Generic;
using TimeReporter.Model;

namespace TimeReporter.Core
{
    public interface IExporter
    {
        public string Name { get; }

        public bool IsEnabled { get; set; }

        public string TemplatePath { get; set; }

        public string OutputDirectory { get; set; }

        public void Export(List<Day> days);
    }
}
