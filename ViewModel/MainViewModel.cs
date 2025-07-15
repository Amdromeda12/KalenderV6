using MvvmHelpers;
using MvvmHelpers.Commands;
using MVVMViewModel.MVVM;
using System.Windows.Input;

namespace KalenderV6.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand PreviewMouseLeftButtonDownCommand => new RelayCommand(execute => ExecutePreviewMouseLeftButtonDown());


        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private bool _isToggleButtonChecked;
        public bool IsToggleButtonChecked
        {
            get => _isToggleButtonChecked;
            set
            {
                if (_isToggleButtonChecked != value)
                {
                    _isToggleButtonChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowContentCommand { get; }
        public ICommand ExitCommand { get; }


        public MainViewModel()
        {
            ShowHomeCommand = new Command(ShowHome);
            ShowContentCommand = new Command(ShowContent);
            ExitCommand = new Command(Exit);

            CurrentViewModel = new HomeViewModel();

        }

        private void ShowHome()
        {
            CurrentViewModel = new HomeViewModel();
        }

        private void ShowContent()
        {
            CurrentViewModel = new ContentViewModel();
        }
        private void ExecutePreviewMouseLeftButtonDown()
        {
            IsToggleButtonChecked = false;
        }
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
