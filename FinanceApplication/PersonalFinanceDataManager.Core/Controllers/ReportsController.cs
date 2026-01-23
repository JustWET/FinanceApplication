using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Core.Abstractions;
using PersonalFinanceDataManager.Core.Services.Interfaces;

namespace PersonalFinanceDataManager.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : MyControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date)
        {
            if (date == default)
                return BadRequest("Invalid date.");

            var userId = GetUserId();
            var report = await _reportsService.GetDailyReportAsync(userId, date);
            return Ok(report);
        }

        [HttpGet("period")]
        public async Task<IActionResult> GetPeriodReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (startDate == default || endDate == default)
                return BadRequest("Invalid date range.");

            if (endDate < startDate)
                return BadRequest("End date must be after start date.");

            var userId = GetUserId();
            var report = await _reportsService.GetPeriodReportAsync(userId, startDate, endDate);
            return Ok(report);
        }
    }
}
