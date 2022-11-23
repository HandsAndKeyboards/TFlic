using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public class GraphAggregator : IGraphAggregator
    {
        #region Public

        public GraphAggregator(string _GraphType, Sprint _Sprint)
        {
            GraphType = _GraphType;
            TeamSprint = _Sprint;
        }

        #region methods

        /// <summary>
        /// формирует массив подписей для графа на оси ох
        /// </summary>
        public void FormXLabels()
        {
            XLabels = new List<string>();
            switch (GraphType)
            {
                case "Burndown":
                    break;
                case "TeamSpeed":
                    break;
                case "UserWorkload":
                    throw new ArgumentException("userworkload diagram do not support this method");
                default:
                    throw new ArgumentException("invalid argument for this method");
            }
        }

        /// <summary>
        /// формирует массив подписей для графа на оси ох
        /// </summary>
        public void FormYValues()
        {
            YValues = new List<double>();
            switch (GraphType)
            {
                case "Burndown":
                    break;
                case "TeamSpeed":
                    break;
                case "UserWorkload":
                    throw new ArgumentException("userworkload diagram do not support this method");
                default:
                    throw new ArgumentException("invalid argument for this method");
            }
        }
        #endregion

        #endregion

        #region private

        #region methods

        /// <summary>
        /// суммирует "примерное время выполнения" задачи, указанное разработчиками
        /// </summary>
        private double AggregateEstimatedTime()
        {
            double aggregatedtime = 0;
            // - todo
            return aggregatedtime;
        }

        /// <summary>
        /// суммирует "реальное время выполнения" задачи, указанное разработчиками
        /// </summary>
        private double AggregateRealTime()
        {
            double aggregatedtime = 0;
            // - todo
            return aggregatedtime;
        }

        #endregion

        #region fields

        private readonly string[] GraphTypes = new string[] { "burndown", "teamspeed", "usersworkload" };

        #endregion

        #endregion

        public IReadOnlyCollection<double> ChartValues { get; set; }
        public IReadOnlyCollection<string> XLabels { get; set; }
        public IReadOnlyCollection<double> YValues { get; set; }
        public string GraphType { get; set; }
        public Sprint TeamSprint { get; set; }
    }
}