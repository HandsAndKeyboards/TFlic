using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization;
using Organization.Models.Organization.Graphs;
using Organization.Models.Organization.Project;

namespace Organization.Controllers
{
    [ApiController]
    [Route("Organizations")]
    public class GraphController : ControllerBase
    {
        /// <summary>
        /// Получить бёрндаун график
        /// </summary>
        /*
             * ------------------TODO---------------------
             * Связть таблицы tasks и logs
             * projectid? 
        */
        [HttpGet("BurndownGraph")]
        public IActionResult BurndownGraph(ulong OrganizationId, ulong ProjectId, ulong sprint_number) 
        {
            // - Получаем значения даты и времени из логов 
            using var LogCtx = DbContexts.Get<LogContext>();
            var estimatedTimes = LogCtx.Logs.Where(obj => ((obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.sprint_number == sprint_number)))
                .Select(obj => obj.new_estimated_time)
                .ToList();
            var dates = LogCtx.Logs.Where(obj => (obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.sprint_number == sprint_number))
                .Select(obj => obj.edit_date)
                .ToList();

            // - Сформировать массив точек на графе
            var chartValues = new List<DateTimePoint>(estimatedTimes.Count);
            bool foundSameDate;
            for (int i = 0; i < estimatedTimes.Count; i++)
            {
                foundSameDate = false;
                // - Пройтись по уже добавленным точкам и изменить на сумму времени, изменения в один день
                for(int j = 0; j < chartValues.Count; j++)
                {
                    if (chartValues[j].Point == dates[i])
                    {
                        chartValues[j] = new DateTimePoint(dates[i], estimatedTimes[j] + estimatedTimes[i]);
                        foundSameDate = true;
                    }
                }
                if(!foundSameDate) chartValues.Add(new DateTimePoint(dates[i], estimatedTimes[i]));
            }

            // - Получаем спринт из бд по номеру
            using var SprintCtx = DbContexts.Get<SprintContext>();
            var sprints = SprintCtx.Sprints.Where(obj => (obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.Number == sprint_number))
                .Select(obj => obj)
                .ToList();
            var sprint = sprints[0];

            var graph = new GraphAggregator("Burndown", sprint, chartValues);

            return Ok(new Graph(graph));
        }

        /// <summary>
        /// Получить значение по оси ОY
        /// </summary>
        [HttpGet("TeamSpeedGraph")]
        public IActionResult GetTeamSpeedGraph(ulong OrganizationId, ulong ProjectId, ulong sprint_begin, ulong sprint_end)
        {
            // - Получаем спринт из бд в указанном диапазоне
            using var SprintCtx = DbContexts.Get<SprintContext>();
            var sprints = SprintCtx.Sprints.Where(obj => (obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.Number <= sprint_end) && (obj.Number >= sprint_begin))
                .ToList();

            // - Сформировать массив точек на графе
            var chartValues = new List<SprintTimePoint>(sprints.Count);

            // - Пройдёмся по каждому спринту и сформируем массив точек
            for (int i = 0; i < sprints.Count; i++)
            {
                // - Получаем значения даты и времени из логов 
                using var LogCtx = DbContexts.Get<LogContext>();
                var estimatedTimes = LogCtx.Logs.Where(obj => ((obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.sprint_number == sprints[i].Number)))
                    .Select(obj => obj.new_estimated_time)
                    .ToList();
                var realTimes = LogCtx.Logs.Where(obj => ((obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.sprint_number == sprints[i].Number)))
                    .Select(obj => obj.real_time)
                    .ToList();
                ulong estimatedTime = estimatedTimes[0], realTime = realTimes[0];
                for (int j = 0; j < estimatedTimes.Count; j++)
                {
                    estimatedTime += estimatedTimes[j];
                    realTime += realTimes[j];
                }
                chartValues.Add(new SprintTimePoint(sprints[i], estimatedTime, realTime));
            }

            var graph = new GraphAggregator("TeamSpeed", sprints.ToArray(), chartValues);

            return Ok(new Graph(graph));
        }

        [HttpGet("UsersWorkloadGraph")]
        public IActionResult GetGraphValues(ulong sprint_number)
        {

            return Ok("Ok");
        }
    }
}
