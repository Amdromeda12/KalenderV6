using KalenderV6.Database;
using KalenderV6.UserControls;
using MvvmHelpers;
using MVVMViewModel.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KalenderV6.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private int day;
        private int month;
        private int year;
        private string _eventText;
        public string EventText
        {
            get => _eventText;
            set
            {
                _eventText = value;
                OnPropertyChanged();
            }
        }

        private string _monthYear;
        public string MonthYear
        {
            get => _monthYear;
            set
            {
                if (_monthYear != value)
                {
                    _monthYear = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand ArrowPastCommand => new RelayCommand(execute => PreviousMonth());
        public RelayCommand ArrowNextCommand => new RelayCommand(execute => NextMonth());
        public ObservableCollection<UserControl> Days { get; set; }
        private TaskControlViewModel _taskControlViewModel;

        public HomeViewModel()
        {

            _taskControlViewModel = new TaskControlViewModel();
            Days = new ObservableCollection<UserControl>();
            DateTime now = DateTime.Now;
            day  = now.Day;
            month = now.Month;
            year = now.Year;
            DisplayDays();
        }

        public void DisplayDays()
        {
            Days.Clear();

            DateTime startOfMonth = new DateTime(year, month, 1);
            MonthYear = $"{startOfMonth:MMMM} {year}";

            int daysInMonth = DateTime.DaysInMonth(year, month);
            int dayOfWeek = (int)startOfMonth.DayOfWeek;

            dayOfWeek = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;

            // Add blank controls for days before the start of the month
            for (int i = 0; i < dayOfWeek; i++)
            {
                Days.Add(new UserControlBlank());
            }

            var events = DatabaseHelper.LoadEventsForMonth(year, month);

            // Add controls for each day in the month
            for (int i = 1; i <= daysInMonth; i++)
            {
                var dayControl = new UserControlDays(this, _taskControlViewModel);
                var dayControlViewModel = (UserControlDaysViewModel)dayControl.DataContext;
                dayControlViewModel.Day = i;
                dayControlViewModel.Month = month;
                dayControlViewModel.Year = year;

                if(events.TryGetValue(new DateTime(year, month, i), out string eventName))
                {
                    dayControlViewModel.EventText = eventName;
                }

                Days.Add(dayControl);
            }
        }
        private void NextMonth()
        {
            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
            DisplayDays();
        }
        private void PreviousMonth()
        {
            month--;
            if (month < 1)
            {
                month = 12;
                year--;
            }
            DisplayDays();
        }
    }
}
