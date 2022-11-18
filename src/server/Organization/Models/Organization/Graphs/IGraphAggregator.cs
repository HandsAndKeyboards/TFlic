using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public interface IGraphAggregator
    {
        /// <summary>
        /// Массив точек которые отмечаются на графе
        /// </summary>
        IReadOnlyCollection<double> ChartValues { get; set; }

        /// <summary>
        /// Даты на оси ОХ => спринты
        /// </summary>
        IReadOnlyCollection<string> XLabels { get; set; }

        /// <summary>
        /// Значения на оси ОY => время | кол-во задач
        /// </summary>
        IReadOnlyCollection<double> YValues { get; set; }

        /// <summary>
        /// Тип графа: Burndown,TeamSpeed, UsersWorkload
        /// </summary>
        string GraphType { get; set; }

        /// <summary>
        /// Тип графа: Burndown,TeamSpeed, UsersWorkload
        /// </summary>
        Sprint TeamSprint { get; set; }
    }
}
