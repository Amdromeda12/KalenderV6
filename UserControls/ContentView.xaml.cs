using KalenderV6.Model;
using KalenderV6.ViewModel;
using System.Windows.Controls;

namespace KalenderV6
{
    public partial class ContentView : UserControl
    {
        public ContentView()
        {
            InitializeComponent();
            this.DataContext = new ContentViewModel();
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Ensure the view model method is called
            if (DataContext is ContentViewModel viewModel)
            {
                viewModel.DataGrid_CellEditEnding(sender, e);
            }
        }
    }
}
