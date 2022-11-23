using Microsoft.AspNetCore.Mvc;
using Organization.Controllers.Service;
using Organization.Models.Contexts;
using Organization.Models.Organization.Project;

namespace Organization.Controllers
{
    [ApiController]
    [Route("Organizations/{OrganizationId}/Project/{ProjectId}")]
    public class GraphController : ControllerBase
    {
        /// <summary>
        /// Получить значение по оси ОХ
        /// </summary>
        [HttpGet("ReportGraphOX")]
        public IActionResult GetGraphOX(Sprint sprint) 
        {
            // TODO

            return ResponseGenerator.Ok("XLabels");
        }

        [HttpGet("ReportGraphOY")]
        public IActionResult GetGraphOY(Sprint sprint)
        {
            // TODO

            return ResponseGenerator.Ok("XLabels");
        }

        [HttpGet("ReportGraphValues")]
        public IActionResult GetGraphValues(Sprint sprint)
        {
            // TODO

            return ResponseGenerator.Ok("XLabels");
        }
    }
}
