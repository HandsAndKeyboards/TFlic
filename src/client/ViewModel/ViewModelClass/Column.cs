using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlic.ViewModel.ViewModelClasses
{
    public class Column
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
    }
}
