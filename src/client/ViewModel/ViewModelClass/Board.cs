using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TFlic.ViewModel.ViewModelClass;

namespace TFlic.ViewModel.ViewModelClass
{
    internal class Board
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Column> columns { get; set; }
    }
}
