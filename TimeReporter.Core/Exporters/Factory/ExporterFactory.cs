using System;
using System.Collections.Generic;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters.Factory
{
    internal class ExporterFactory : IExporterFactory
    {
        public IEnumerable<IExporter> GetExporters(List<ExporterDto> storedExporters)
        {
            var type = typeof(IExporter);
            var allImplementations = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.IsClass)
                .Select(p => new ExporterDto() { TypeName = p.FullName });

            //foreach (var implementation in allImplementations)
            //{
            //    var match = storedExporters.FirstOrDefault(x => x.TypeName == implementation.TypeName);
            //    if (match != null)
            //    {
            //        implementation.IsEnabled = match.IsEnabled;
            //        implementation.OutputDirectory = match.OutputDirectory;
            //        implementation.TemplatePath = match.TemplatePath;
            //    }
            //}

            var result = new List<IExporter>();
            foreach (var dto in allImplementations)
            {
                var exporterInstance = (IExporter)Activator.CreateInstance(GetType().Assembly.FullName, dto.TypeName).Unwrap();
                var match = storedExporters.FirstOrDefault(x => x.TypeName == dto.TypeName);

                exporterInstance.IsEnabled = match?.IsEnabled ?? dto.IsEnabled;
                exporterInstance.TemplatePath = match?.TemplatePath ?? dto.TemplatePath;
                exporterInstance.OutputDirectory = match?.OutputDirectory ?? dto.OutputDirectory;

                result.Add(exporterInstance);
            }

            return result;
        }
    }
}
