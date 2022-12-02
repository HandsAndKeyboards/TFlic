using Organization.Models.Organization.Project;

namespace Organization.Models.Organization.Graphs
{
    public interface IGraphAggregator
    {
        /// <summary>
        /// Массив точек которые отмечаются на графе
        /// </summary>
        IReadOnlyCollection<DateTimePoint> DateChartValues { get; set; }

        /// <summary>
        /// Массив точек которые отмечаются на графе
        /// </summary>
        IReadOnlyCollection<SprintTimePoint> SprintChartValues { get; set; }

        /// <summary>
        /// Тип графа: Burndown,TeamSpeed, UsersWorkload
        /// </summary>
        string GraphType { get; set; }

        /// <summary>
        /// Спринт на основе которого строится бёрндаун
        /// </summary>
        Sprint TeamSprint { get; set; }

        /// <summary>
        /// Спринты на основе которого строится тимспид
        /// </summary>
        Sprint[] TeamSprints { get; set; }
    }
}
