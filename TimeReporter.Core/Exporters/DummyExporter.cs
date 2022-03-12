using System;
using System.Collections.Generic;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    internal class DummyExporter : IExporter
    {
        public string Name { get; } = "Dummy Exporter";

        public bool IsEnabled { get; set; }
        public string TemplatePath { get; set; }
        public string OutputDirectory { get; set; }

        public void Export(List<Day> days)
        {
            throw new NotImplementedException();
        }
    }
}
