﻿using Organization.Models.Organization.Project;
using System.Collections.ObjectModel;

namespace Organization.Models.Organization.Graphs
{
    public class GraphAggregator : IGraphAggregator
    {
        #region Public

        public GraphAggregator(string _GraphType, Sprint _Sprint, List<DateTimePoint> _ChartValues)
        {
            GraphType = _GraphType;
            TeamSprint = _Sprint;
            ChartValues = _ChartValues;
        }

        #region methods

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

        public IReadOnlyCollection<DateTimePoint> ChartValues { get; set; }
        public string GraphType { get; set; }
        public Sprint TeamSprint { get; set; }
    }
}