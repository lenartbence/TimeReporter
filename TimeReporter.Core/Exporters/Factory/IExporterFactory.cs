using System.Collections.Generic;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters.Factory
{
    public interface IExporterFactory
    {
        IEnumerable<IExporter> GetExporters(List<ExporterDto> dtos);
    }
}