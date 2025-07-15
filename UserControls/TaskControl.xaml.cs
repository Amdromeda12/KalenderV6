using KalenderV6.Model;
using KalenderV6.ViewModel;
using MVVMViewModel.MVVM;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace KalenderV6.UserControls
{
    public partial class TaskControl : UserControl
    {
        public TaskControl()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TODO: TestKod
            var viewModel = DataContext as TaskControlViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedItem = e.NewValue;
            }
        }
    }
}
