using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Controllers.DTO;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
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
        [HttpGet("BurndownGraph")]
        public IActionResult BurndownGraph(ulong OrganizationId, ulong ProjectId, ulong sprint_number) 
        {
            /*
             * ------------------TODO---------------------
             * Связть таблицы tasks и logs
             * projectid? 
            */
            // - Получаем значения даты и времени из логов 
            using var LogCtx = DbContexts.Get<LogContext>();
            var estimatedTimes = LogCtx.Logs.Where(obj => ((obj.OrganizationId == OrganizationId) && (obj.ProjectId == ProjectId) && (obj.sprint_number == sprint_number)))
                .Select(obj => obj.new_estimated_time)
                .ToList();
            var dates = LogCtx.Logs.Where(obj => obj.sprint_number == sprint_number)
                .Select(obj => obj.edit_date)
                .ToList();

            // - Сформировать массив точек на графе
            var chartValues = new List<DateTimePoint>(estimatedTimes.Count);
            for (int i = 0; i < estimatedTimes.Count; i++)
            {
                chartValues.Add(new DateTimePoint(dates[i], estimatedTimes[i]));
            }

            /*
             * ------------------TODO---------------------
             * Добавить таблицу(?) с информацией о спринте
             * и брать инфу для класса спринт оттудова
            */
            var sprint = new Sprint();
            var graph = new GraphAggregator("Burndown", sprint, chartValues);

            return ResponseGenerator.Ok(value: new Graph(graph));
        }

        /// <summary>
        /// Получить значение по оси ОY
        /// </summary>
        [HttpGet("TeamSpeedGraph")]
        public IActionResult GetTeamSpeedGraph(ulong sprint_number)
        {

            return ResponseGenerator.Ok("Ok");
        }

        [HttpGet("UsersWorkloadGraph")]
        public IActionResult GetGraphValues(ulong sprint_number)
        {

            return ResponseGenerator.Ok("Ok");
        }
    }
}
