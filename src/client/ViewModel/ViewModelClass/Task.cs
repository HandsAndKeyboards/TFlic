using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Task
    {
        public int Id { get; set; }
        public int IdColumn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Brush ColorPriority { get; set; }
        public int ExecutionTime { get; set; }
        public DateTime DeadLine { get; set; }
        public string NameExecutor { get; set; } 
    }
}
