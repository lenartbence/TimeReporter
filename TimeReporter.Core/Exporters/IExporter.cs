using System.Collections.Generic;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    public interface IExporter
    {
        string Name { get; }

        bool IsEnabled { get; set; }

        string TemplatePath { get; set; }

        string Message { get; set; }

        void Export(List<Day> days);
    }
}
