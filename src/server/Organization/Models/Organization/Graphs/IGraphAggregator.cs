using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public interface IGraphAggregator
    {
        /// <summary>
        /// Массив точек которые отмечаются на графе
        /// </summary>
        IReadOnlyCollection<DateTimePoint> ChartValues { get; set; }

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
