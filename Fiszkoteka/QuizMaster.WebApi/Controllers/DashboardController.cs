using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Dto;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.WebApi.Extensions;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/dashboard")]
    public sealed class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MainDashboardDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDashboard(
            CancellationToken cancellationToken)
        {
            var result = await _dashboardService.GetDashboard(
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }
    }
}
