using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeReporter.Core.Exporters;
using TimeReporter.Core.Exporters.Factory;
using TimeReporter.Core.Storage;
using TimeReporter.Model;
using TimeReporter.UI.Commands;
using TimeReporter.UI.Models;

namespace TimeReporter.UI.ViewModels
{
    public class MainWindowViewModel : AbstractViewModel
    {
        private readonly IStorageManager<Day, DateTime> _dayStorage;
        private readonly IStorageManager<ExporterDto> _exporterStorage;
        private readonly IStorageReader<Day, DateTime> _holidayReader;

        private DateTime _currentMonth;
        private ObservableCollection<SelectableDay> _days;
        private string _bottomMessage;

        public MainWindowViewModel(
            IStorageManager<Day, DateTime> dayStorage,
            IStorageManager<ExporterDto> exporterStorage,
            IStorageReader<Day, DateTime> holidayReader,
            IExporterFactory exporterFactory)
        {
            InitializeCommands();

            _dayStorage = dayStorage;
            _exporterStorage = exporterStorage;
            _holidayReader = holidayReader;

            var storedExporters = _exporterStorage.Load().ToList();
            Exporters = exporterFactory.GetExporters(storedExporters).Select(x => new NotifierExporter(x)).ToList();

            CurrentMonth = DateTime.Now;
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

        public string BottomMessage
        {
            get => _bottomMessage;
            set => Set(ref _bottomMessage, value);
        }

        public string UserName { get; set; }

        public List<string> Projects { get; set; } = new List<string>() { "Weekend", "National Holiday", "Day Off", "OD Mutterschutz", "OD Kündigung Schwerbehinderte" };

        public string SelectedDayType { get; set; }

        public List<NotifierExporter> Exporters { get; set; }

        public RelayCommand NextMonthCommand { get; set; }

        public RelayCommand PreviousMonthCommand { get; set; }

        public RelayCommand ApplyDayTypeCommand { get; set; }

        public SelectDayCommand SelectDayCommand { get; set; }

        public RelayCommand SelectAllWorkdaysCommand { get; set; }

        public RelayCommand DeselectAllCommand { get; set; }

        public RelayCommand ExportCommand { get; set; }

        private void LoadDays(DateTime target)
        {
            BottomMessage = string.Empty;

            IEnumerable<Day> content = _dayStorage.Load(target);
            IEnumerable<Day> specialDays = _holidayReader.Load(target);
            if (!content.Any())
            {
                content = Enumerable.Range(1, DateTime.DaysInMonth(target.Year, target.Month))
                                      .Select(day =>
                                      {
                                          DateTime date = new DateTime(target.Year, target.Month, day);
                                          return new Day()
                                          {
                                              Date = date,
                                              Type = GetDayType(date, specialDays)
                                          };
                                      });
            }

            if (specialDays == null || !specialDays.Any())
            {
                BottomMessage = "Could not load national holidays. Please review manually.";
            }

            InitializeDays(content);
        }

        private void InitializeDays(IEnumerable<Day> content)
        {
            Days = new ObservableCollection<SelectableDay>(content.Select(x => new SelectableDay()
            {
                IsSelected = false,
                Date = x.Date,
                Project = x.Project,
                Type = x.Type
            }));
        }

        private static DayType GetDayType(DateTime date, IEnumerable<Day> specialDays)
        {
            var result = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday ? DayType.Weekend : DayType.Work;

            var special = specialDays?.FirstOrDefault(x => x.Date == date);
            if (special != null)
            {
                if (special.Type == DayType.Work && result == DayType.Weekend)
                {
                    result = DayType.Work;
                }
                else if (special.Type == DayType.NationalHoliday && result == DayType.Work)
                {
                    result = DayType.NationalHoliday;
                }
            }

            return result;
        }

        private void InitializeCommands()
        {
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

                    // TODO: Eliminate magic string
                    if (SelectedDayType == "National Holiday")
                    {
                        day.Type = DayType.NationalHoliday;
                    }
                    else if (SelectedDayType == "Day Off")
                    {
                        day.Type = DayType.DayOff;
                    }
                    else if (SelectedDayType == "Weekend")
                    {
                        day.Type = DayType.Weekend;
                    }
                    else
                    {
                        day.Type = DayType.Work;
                    }

                    day.Project = day.Type == DayType.Weekend ? string.Empty : SelectedDayType;
                }
                Days = new ObservableCollection<SelectableDay>(temp);
                _dayStorage.Save(Days);

                DeselectAllCommand.Execute(null);
            });

            SelectDayCommand = new SelectDayCommand();

            SelectAllWorkdaysCommand = new RelayCommand(() =>
            {
                foreach (var d in Days)
                {
                    if (d.Type == DayType.Work)
                    {
                        d.IsSelected = true;
                    }
                }
            });

            DeselectAllCommand = new RelayCommand(() =>
            {
                foreach (var d in Days)
                {
                    d.IsSelected = false;
                }
            });

            ExportCommand = new RelayCommand(() =>
            {
                SaveExporters();

                foreach (var e in Exporters)
                {
                    if (e.IsEnabled)
                    {
                        e.Export(Days.Select(x => x as Day).ToList());
                    }
                }
            });
        }

        private void SaveExporters()
        {
            var dtos = Exporters.Select(x => new ExporterDto()
            {
                IsEnabled = x.IsEnabled,
                TemplatePath = x.TemplatePath,
                TypeName = x.TypeName
            });

            _exporterStorage.Save(dtos);
        }
    }
}
