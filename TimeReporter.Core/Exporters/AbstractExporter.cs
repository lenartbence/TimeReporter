using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimeReporter.Model;

namespace TimeReporter.Core.Exporters
{
    internal abstract class AbstractExporter : IExporter
    {
        private string _message;

        public virtual string Name { get; }

        public bool IsEnabled { get; set; }

        public string TemplatePath { get; set; }

        public string Message
        {
            get => _message;
            set => _message = $"({DateTime.Now.ToShortTimeString()}) {value}";
        }

        public virtual void Export(List<Day> days)
        {
            if (days == null || !days.Any())
            {
                Message = "Error processing days.";
            }

            if (!File.Exists(TemplatePath))
            {
                Message = "Template not found.";
                return;
            }
        }
    }
}
