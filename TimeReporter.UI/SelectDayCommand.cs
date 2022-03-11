using System;
using System.Windows.Input;

namespace TimeReporter.UI
{
    class SelectDayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter is SelectableDay;
        }

        public void Execute(object parameter)
        {
            var day = (SelectableDay)parameter;
            day.IsSelected = !day.IsSelected;
        }
    }
}
