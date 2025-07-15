using MvvmHelpers;
using MVVMViewModel.MVVM;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SQLite;
using KalenderV6.Database;
using KalenderV6.View;
using KalenderV6.Model;


namespace KalenderV6.ViewModel
{
    public class UserControlDaysViewModel : BaseViewModel
    {
        private HomeViewModel _homeViewModel;

        private int _day;
        public int Day
        {
            get { return _day; }
            set
            {
                if (SetProperty(ref _day, value))
                {
                    UpdateButtonBackgroundColor();
                }
            }
        }

        private int _month;
        public int Month
        {
            get { return _month; }
            set
            {
                if (SetProperty(ref _month, value))
                {
                    UpdateButtonBackgroundColor();
                }
            }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                if (SetProperty(ref _year, value))
                {
                    UpdateButtonBackgroundColor();
                }
            }
        }

        private Brush _buttonBackgroundColor;
        public Brush ButtonBackgroundColor
        {
            get => _buttonBackgroundColor;
            set => SetProperty(ref _buttonBackgroundColor, value);
        }

        private string _eventText;
        public string EventText
        {
            get => _eventText;
            set => SetProperty(ref _eventText, value);
        }
        public Item Item { get; set; }
        public ICommand DayBtnCommand { get; }
        private readonly TaskControlViewModel _taskControlViewModel;

        public UserControlDaysViewModel(HomeViewModel homeViewModel, TaskControlViewModel taskControlViewModel)
        {
            _homeViewModel = homeViewModel;
            _taskControlViewModel = taskControlViewModel;
            DayBtnCommand = new RelayCommand(ExecuteDayBtnCommand);
            Item = new Item { };
        }

        private void ExecuteDayBtnCommand(object obj)
        {
            Item.StartDate = new DateTime(Year, Month, Day);

            var mainWindow = Application.Current.MainWindow;
            var viewModel = new CreateTaskViewModel(mainWindow, Item);
            var window = new CreateTask(viewModel)
            {
                Owner = mainWindow
            };

            window.ShowDialog();

            if (window.Success)
            {
                DatabaseHelper.InsertRecord(Item.Name, Item.TaskType, Item.StartDate, Item.Time, Item.Howlong, Item.Url, Item.IsCompleted);
            }
        }

        private void UpdateButtonBackgroundColor()
        {
            if (!IsValidDate(Year, Month, Day))
            {
                return;
            }

            DateTime viewModelDate = new DateTime(Year, Month, Day);
            DateTime today = DateTime.Today;

            ButtonBackgroundColor = viewModelDate.CompareTo(today) switch
            {
                > 0 => Brushes.Yellow,
                0 => Brushes.Red,
                _ => Brushes.White
            };
        }

        private bool IsValidDate(int year, int month, int day)
        {
            return year >= 1 && year <= 9999 &&
                   month >= 1 && month <= 12 &&
                   day >= 1 && day <= DateTime.DaysInMonth(year, month);
        }
    }
}
