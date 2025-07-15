using Microsoft.VisualBasic;

namespace KalenderV6.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaskType { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Time { get; set; }
        public int Howlong { get; set; }
        public bool IsCompleted { get; set; }
    }
}