namespace Organization.Models.Organization.Graphs
{
    public class GraphAggregator : IGraphAggregator
    {
        #region Public

        #endregion

        #region Private

        #endregion

        public IReadOnlyCollection<double> ChartValues { get; init; }
        public IReadOnlyCollection<string> XLabels { get; init; }
        public IReadOnlyCollection<int> YValues { get; init; }
    }
}
