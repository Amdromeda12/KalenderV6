using KalenderV6.ViewModel;
using System.Windows;

namespace KalenderV6.View
{
    public partial class CreateTask : Window
    {
        internal CreateTask(CreateTaskViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Owner = Application.Current.MainWindow;

            viewModel.RequestClose += ViewModel_RequestClose;
        }
        private void ViewModel_RequestClose()
        {
            if (DataContext is CreateTaskViewModel viewModel)
            {
                //föra över ifrån CreateTaskViewModel hit sen till UserControlDaysViewModel
                //Så det går            HÄR!
                //CreateTaskViewModel > CreateTask.cs > UserControlDaysViewModel

                Success = viewModel.Success;
            }
            this.Close();
        }
        public bool Success { get; private set; }
    }
}
