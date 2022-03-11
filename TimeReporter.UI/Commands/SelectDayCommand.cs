using System;
using System.Windows.Input;
using TimeReporter.UI.Models;

namespace TimeReporter.UI.Commands
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
