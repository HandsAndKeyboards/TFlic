using Microsoft.AspNetCore.Mvc;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers
{
    [ApiController]
    [Route("Organizations")]
    public class SprintController : Controller
    {
        [HttpGet("Sprints")]
        public ActionResult<IEnumerable<Sprint>> GetSprints(ulong OrganizationId, ulong ProjectId) 
        {
            using var SprintCtx = DbContexts.Get<SprintContext>();
            
            var sprints = SprintCtx.Sprints.Where(x => (x.OrganizationId == OrganizationId) && (x.ProjectId == ProjectId))
/*                .Include(x => x)*/
                .ToList();

/*            var Dto = projects.Select(x => new DTO.Project(x)).ToList();*/
            return Ok(sprints);
        }
    }
}
