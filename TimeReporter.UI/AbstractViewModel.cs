using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeReporter.UI
{
    internal abstract class AbstractViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string caller = null)
        {
            if (field == null || !field.Equals(value))
            {
                field = value;
                NotifyPropertyChanged(new PropertyChangedEventArgs(caller));
                return true;
            }

            return false;
        }

        protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
