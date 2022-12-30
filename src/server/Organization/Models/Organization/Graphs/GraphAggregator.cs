using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public class GraphAggregator
    {
        #region Public

        public GraphAggregator(string _GraphType, Sprint _Sprint, List<DateTimePoint> _ChartValues)
        {
            GraphType = _GraphType;
            TeamSprint = _Sprint;
            DateChartValues = _ChartValues;
        }

        public GraphAggregator(string _GraphType, Sprint[] _Sprints, List<SprintTimePoint> _ChartValues)
        {
            GraphType = _GraphType;
            TeamSprints = _Sprints;
            SprintChartValues = _ChartValues;
        }

        #region methods

        #endregion

        #endregion

        #region private

        #region methods
        #endregion

        #region fields
        #endregion

        #endregion

        public IReadOnlyCollection<DateTimePoint> DateChartValues { get; set; }
        public IReadOnlyCollection<SprintTimePoint> SprintChartValues { get; set; }
        public string GraphType { get; set; }
        public Sprint TeamSprint { get; set; }
        public Sprint[] TeamSprints { get; set; }
    }
}