using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlic.Model;

namespace TFlic.ViewModel.ViewModelClass
{
    public class Graph
    {
        public Graph(List<DateTimePoint> dateChartValues, string graphType)
        {
            DateChartValues = dateChartValues;
            SprintChartValues = null;
            GraphType = graphType;
        }

        public Graph(List<SprintTimePoint> sprintChartValues, string graphType)
        {
            SprintChartValues = sprintChartValues;
            DateChartValues = null;
            GraphType = graphType;
        }

        public List<DateTimePoint> DateChartValues { get; }
        public List<SprintTimePoint> SprintChartValues { get; }
        public string GraphType { get; }
    }
}
