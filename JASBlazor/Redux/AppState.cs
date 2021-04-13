using JASBlazor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JASBlazor.Redux
{
    public class AppState
    {
        public string Location { get; set; }
        public int Count { get; set; }

        public bool IsLoading { get; set; }

        public IEnumerable<ToDoItem> ToDos { get; set; }

        public ObservableCollection<Issue> Issues { get; set; }
    }


}
