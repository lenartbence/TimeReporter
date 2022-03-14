using System.Collections.Generic;
using System.IO;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    internal abstract class AbstractExporter : IExporter
    {
        public virtual string Name { get; }

        public virtual bool IsEnabled { get; set; }

        public virtual string TemplatePath { get; set; }

        public virtual string Message { get; set; }

        public virtual void Export(List<Day> days)
        {
            if (!File.Exists(TemplatePath))
            {
                Message = "Template not found.";
                return;
            }
        }
    }
}
