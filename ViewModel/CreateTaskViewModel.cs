using KalenderV6.Model;
using MVVMViewModel.MVVM;
using System;
using System.Diagnostics;
using System.Windows;

namespace KalenderV6.ViewModel
{
    internal class CreateTaskViewModel : ViewModelBase
    {
        public RelayCommand PressBtnClose { get; }
        public RelayCommand PressBtnOk { get; }

        private bool _success;
        public bool Success
        {
            get => _success;
            set
            {
                if (_success != value)
                {
                    _success = value;
                    OnPropertyChanged();
                }
            }
        }

        private Window _owner;
        public Window Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged();
                }
            }
        }
        public Item Item { get; }

        public CreateTaskViewModel(Window parentWindow, Item item)
        {
            Owner = parentWindow;
            Item = item;

            PressBtnClose = new RelayCommand(execute => BtnClose());
            PressBtnOk = new RelayCommand(execute => BtnOk());
        }

        public event Action RequestClose;
        private void BtnOk()
        {
            if (string.IsNullOrEmpty(Item.Name) || string.IsNullOrEmpty(Item.TaskType))
            {
                MessageBox.Show("Name and Task Type are required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Success = true;

            RequestClose?.Invoke();
        }

        private void BtnClose()
        {
            RequestClose?.Invoke();
        }
    }
}
