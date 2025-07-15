using MvvmHelpers;
using KalenderV6.Database;
using System.Collections.ObjectModel;
using System.Linq;
using KalenderV6.Model;
using System.Threading.Tasks;
using System.Diagnostics;
using MVVMViewModel.MVVM;
using System.Windows;
using System.Windows.Controls;
using System;

namespace KalenderV6.ViewModel
{
    public class TaskControlViewModel : BaseViewModel
    {
        public RelayCommand BtnCompletePress => new RelayCommand(execute => Complete());
        public RelayCommand BtnWatchPress => new RelayCommand(execute => Watch());

        private ObservableCollection<ParentTaskItem> tasks;
        public ObservableCollection<ParentTaskItem> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value);
        }

        private object selectedItem;
        public object SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public TaskControlViewModel()
        {
            LoadTaskItems();
        }

        public void LoadTaskItems()
        {
            var allEvents = DatabaseHelper.LoadAllEvents();

            var parentTaskItems = new List<ParentTaskItem>();

            foreach (var eventGroup in allEvents)
            {
                string baseName = eventGroup.Key;

                // Create the parent TaskItem
                var parentTaskItem = new ParentTaskItem
                {
                    Name = baseName,
                    IsExpanded = false
                };

                int count = 1;
                foreach (var eventDetail in eventGroup.Value)
                {
                    // Create a unique name by appending the count
                    string uniqueName = $"{baseName} {count}";

                    // Add the child TaskItem to the parent's Children collection
                    parentTaskItem.Children.Add(new ChildTaskItem
                    {
                        Name = uniqueName,
                        Date = eventDetail.Date,
                        IsCompleted = eventDetail.IsCompleted,
                        Id = eventDetail.Id,
                        Url = eventDetail.Url
                    });

                    count++;
                }
                parentTaskItems.Add(parentTaskItem);
            }
            // Update the Tasks property with the new list of parent TaskItem objects
            Tasks = new ObservableCollection<ParentTaskItem>(parentTaskItems);
        }
        private void Complete()
        {
            if (SelectedItem is ChildTaskItem selectedItem)
            {
                DatabaseHelper.CompleteEvent(selectedItem.Id.ToString());
                selectedItem.IsCompleted = !selectedItem.IsCompleted;
            }
        }
        private void Watch()
        {
            if (SelectedItem is ChildTaskItem selectedItem)
            {
                try
                {
                    string Url = selectedItem.Url;
                    var psi = new ProcessStartInfo
                    {
                        FileName = Url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
