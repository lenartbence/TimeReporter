using System.Collections.Generic;
using System.ComponentModel;
using TimeReporter.Core.Exporters;
using TimeReporter.Model;

namespace TimeReporter.UI.Models
{
    public class NotifierExporter : IExporter, INotifyPropertyChanged
    {
        private IExporter _internalExporter;

        public NotifierExporter(IExporter internalExporter)
        {
            _internalExporter = internalExporter;
        }

        public string Name => _internalExporter.Name;

        public bool IsEnabled
        {
            get => _internalExporter.IsEnabled;
            set => _internalExporter.IsEnabled = value;
        }

        public string TemplatePath
        {
            get => _internalExporter.TemplatePath;
            set => _internalExporter.TemplatePath = value;
        }

        public string Message
        {
            get => _internalExporter.Message;
            set => _internalExporter.Message = value;
        }

        public string TypeName => _internalExporter.GetType().FullName;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Export(List<Day> days)
        {
            _internalExporter.Export(days);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
        }
    }
}
