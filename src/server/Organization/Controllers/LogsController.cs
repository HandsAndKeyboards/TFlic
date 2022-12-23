using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.DTO.GET;
using Organization.Controllers.DTO.POST;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers
{
    [ApiController]
    [Route("Organizations/Projects/Boards/Columns")]
    public class LogsController : Controller
    {
        [HttpPost("Logs")]
        public ActionResult<Log> PostLogs(
            ulong OrganizationId, 
            ulong ProjectId, 
            ulong BoardId, 
            ulong ColumnId, 
            ulong AccountId, 
            ulong NewTime,
            ulong RealTime,
            uint  SprintNumber,
            ulong EstimatedTime,
            ulong TaskId)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            if (!PathChecker.IsTaskPathCorrect(OrganizationId, ProjectId, BoardId, ColumnId))
                return NotFound();

            using var ctx = DbContexts.Get<LogContext>();
            var obj = new Log()
            {
                accountChanged_id = AccountId,
                edit_date = DateTime.Today,
                new_estimated_time = NewTime,
                old_estimated_time = EstimatedTime,
                OrganizationId = OrganizationId,
                ProjectId = ProjectId,
                real_time = RealTime,
                sprint_number = SprintNumber,
                TaskId = TaskId
            };
            ctx.Logs.Add(obj);
            ctx.SaveChanges();
            return Ok(obj);
        }
    }
}
