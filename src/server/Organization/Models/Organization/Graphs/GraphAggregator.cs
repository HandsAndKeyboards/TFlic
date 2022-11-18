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

        #region Methods

        /// <summary>
        /// Суммирует "примерное время выполнения" задачи, указанное разработчиками
        /// </summary>
        /// <param name="sprint">Спринт на основе которого строится график</param>
        public double AggregateEstimatedTime()
        {
            double AggregatedTime = 0;
            // - TODO
            return AggregatedTime;
        }

        /// <summary>
        /// Суммирует "реальное время выполнения" задачи, указанное разработчиками
        /// </summary>
        /// <param name="sprint">Спринт на основе которого строится график</param>
        public double AggregateRealTime()
        {
            double AggregatedTime = 0;
            // - TODO
            return AggregatedTime;
        }

        /// <summary>
        /// Формирует массив подписей для графа на оси ОХ
        /// </summary>
        /// <param name="sprint">Спринт на основе которого строится график</param>
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
                    throw new ArgumentException("UserWorkload diagram do not support this method");
                default:
                    throw new ArgumentException("Invalid argument for this method");
            }
        }

        /// <summary>
        /// Формирует массив подписей для графа на оси ОХ
        /// </summary>
        /// <param name="sprint">Спринт на основе которого строится график</param>
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
                    throw new ArgumentException("UserWorkload diagram do not support this method");
                default:
                    throw new ArgumentException("Invalid argument for this method");
            }
        }
        #endregion

        #region Fields
        #endregion
        #endregion

        #region Private

        #region Methods

        #endregion

        #region Fields

        private readonly string[] GraphTypes = { "Burndown", "TeamSpeed", "UsersWorkload" }; 
        
        #endregion
        
        #endregion

        public IReadOnlyCollection<double> ChartValues { get; set; }
        public IReadOnlyCollection<string> XLabels { get; set; }
        public IReadOnlyCollection<double> YValues { get; set; }
        public string GraphType { get; set; }
        public Sprint TeamSprint { get; set; }
    }
}