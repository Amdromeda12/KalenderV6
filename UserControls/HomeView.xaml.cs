using KalenderV6.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace KalenderV6
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            HomeViewModel viewModel = new HomeViewModel();
            this.DataContext = viewModel;
        }
    }
}
