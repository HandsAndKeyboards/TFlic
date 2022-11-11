namespace Organization.Models.Organization.Graphs
{
    public interface IGraphAggregator
    {
        /// <summary>
        /// Массив точек которые отмечаются на графе
        /// </summary>
        IReadOnlyCollection<double> ChartValues { get; init; }

        /// <summary>
        /// Даты на оси ОХ => спринты
        /// </summary>
        IReadOnlyCollection<string> XLabels { get; init; }

        /// <summary>
        /// Значения на оси ОY => время | кол-во задач
        /// </summary>
        IReadOnlyCollection<int> YValues { get; init; }
    }
}
