using KalenderV6.ViewModel;
using System.Windows.Controls;

namespace KalenderV6.UserControls
{
    public partial class UserControlDays : UserControl
    {
        public UserControlDays(HomeViewModel homeViewModel, TaskControlViewModel taskControlViewModel)
        {
            InitializeComponent();
            this.DataContext = new UserControlDaysViewModel(homeViewModel, taskControlViewModel);
        }

        public int Day
        {
            get => ((UserControlDaysViewModel)DataContext).Day;
            set => ((UserControlDaysViewModel)DataContext).Day = value;
        }
    }
}