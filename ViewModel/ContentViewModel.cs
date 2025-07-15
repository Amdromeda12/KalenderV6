using KalenderV6.Database;
using KalenderV6.Model;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KalenderV6.ViewModel
{
    public class ContentViewModel : BaseViewModel
    {
        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ContentViewModel()
        {
            LoadData();
            DeleteItemCommand = new Command<Item>(DeleteItem);
        }

        private void LoadData()
        {
            var events = DatabaseHelper.GetAllEvents();
            Items = new ObservableCollection<Item>(events.Select(e => new Item
            {
                Id = e.Id,
                Name = e.Name,
                TaskType = e.TaskType,
                Url = e.Url,
                StartDate = e.StartDate,
                Time = e.Time, // Ensure this field is included if it’s in your model
                IsCompleted = e.IsCompleted
            }));
        }

        public ICommand DeleteItemCommand { get; }

        public void UpdateItem(Item item)
        {
            DatabaseHelper.UpdateDatabase(item);
        }
        private void DeleteItem(Item item)
        {
            if (item != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Items.Remove(item);

                    DatabaseHelper.DeleteItem(item.Id);
                }
            }
        }

        public void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Ensure the edit action is a commit and the row is not null
            if (e.EditAction == DataGridEditAction.Commit && e.Row != null)
            {
                var item = e.Row.DataContext as Item;
                if (item != null)
                {
                    // Show confirmation dialog before updating
                    MessageBoxResult result = MessageBox.Show("Do you want to save the changes to this item?", "Confirm Update", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        UpdateItem(item);
                    }
                    else
                    {
                        // Revert changes if the user cancels
                        // This will trigger a refresh of the data to undo changes
                        LoadData();
                    }
                }
            }
        }
    }
}
