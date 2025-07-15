using KalenderV6.ViewModel;
using System.Windows;

namespace KalenderV6
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel vm = new MainViewModel();
            DataContext = vm;
        }

        private void nav_panel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}