using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlic.ViewModel.ViewModelClasses
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int ExecutionTime { get; set; }
        public DateTime DeadLine { get; set; }
        public string NameExecutor { get; set; } 
    }
}
