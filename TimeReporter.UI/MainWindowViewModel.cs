using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TimeReporter.Core;
using TimeReporter.Model;

namespace TimeReporter.UI
{
    class MainWindowViewModel : AbstractViewModel
    {
        public MainWindowViewModel()
        {
            CurrentMonth = DateTime.Now;

            NextMonthCommand = new RelayCommand(() => CurrentMonth = CurrentMonth.AddMonths(1));
            PreviousMonthCommand = new RelayCommand(() => CurrentMonth = CurrentMonth.AddMonths(-1));
            ApplyDayTypeCommand = new RelayCommand(() =>
            {
                if (SelectedDayType == null)
                {
                    return;
                }

                var temp = Days.ToList();
                foreach (var day in temp)
                {
                    if (!day.IsSelected)
                        continue;

                    if (day.Type != DayType.Weekend)
                    {
                        // TODO: Eliminate magic string
                        if (SelectedDayType == "National Holiday")
                        {
                            day.Type = DayType.NationalHoliday;
                        }
                        else if (SelectedDayType == "Day Off")
                        {
                            day.Type = DayType.DayOff;
                        }
                        else
                        {
                            day.Type = DayType.Work;
                        }
                        day.Project = SelectedDayType;
                    }
                }
                Days = new ObservableCollection<SelectableDay>(temp);
            });
            SelectDayCommand = new SelectDayCommand();
        }

        public DateTime CurrentMonth
        {
            get
            {
                return _currentMonth;
            }
            set
            {
                if (Set(ref _currentMonth, value))
                {
                    LoadDays(value);
                }
            }
        }

        public ObservableCollection<SelectableDay> Days
        {
            get => _days;
            set => Set(ref _days, value);
        }

        public string UserName { get; set; }

        public List<string> Projects { get; set; } = new List<string>() { "National Holiday", "Day Off", "OD Mutterschutz", "OD Kündigung Schwerbehinderte" };

        public string SelectedDayType { get; set; }

        public List<IExporter> Exporters { get; set; } = new List<IExporter>() { new DummyExporter() };

        public RelayCommand NextMonthCommand { get; set; }

        public RelayCommand PreviousMonthCommand { get; set; }

        public RelayCommand ApplyDayTypeCommand { get; set; }

        public SelectDayCommand SelectDayCommand { get; set; }

        private DateTime _currentMonth;
        private ObservableCollection<SelectableDay> _days;

        private void LoadDays(DateTime target)
        {
            // TODO: Load from file OR initialize
            InitializeDays(target);
        }

        private void InitializeDays(DateTime target)
        {
            Days = new ObservableCollection<SelectableDay>(Enumerable.Range(1, DateTime.DaysInMonth(target.Year, target.Month))
                                    .Select(day =>
                                    {
                                        DateTime date = new DateTime(target.Year, target.Month, day);
                                        return new SelectableDay()
                                        {
                                            Date = date,
                                            Type = GetDayType(date),
                                            IsSelected = false
                                        };
                                    }));
        }

        private static DayType GetDayType(DateTime date)
        {
            // TODO: Get national holidays
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday ? DayType.Weekend : DayType.Work;
        }
    }

    class DummyExporter : IExporter
    {
        public string Name => "Dummy";

        public string TemplatePath { get; set; }

        public string OutputDirectory { get; set; }

        public bool IsEnabled { get; set; }

        public void Export(List<Day> days)
        {
            throw new NotImplementedException();
        }
    }
}
