using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace KalenderV6.Model
{
    public class ParentTaskItem 
    {
        public string Name { get; set; }
        public bool IsExpanded { get; set; }
        public ObservableCollection<ChildTaskItem> Children { get; set; }

        public ParentTaskItem()
        {
            Children = new ObservableCollection<ChildTaskItem>();
        }

        public int IncompleteCountBeforeToday => Children.Count(c => !c.IsCompleted && c.Date.Date < DateTime.Today);
    }

    public class ChildTaskItem : BaseViewModel
    {
        private bool isCompleted;
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }
    }
}
