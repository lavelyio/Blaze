using System;

namespace JASBlazor.Models
{
    public class ToDoItem
    {
        public ToDoItem()
        {
            Id = new Guid();
            Done = false;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public DateTime? Date { get; set; }
        public bool Done { get; set; }
    }
}