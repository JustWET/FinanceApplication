using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.DTOs;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using System.Security.Claims;

namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportsService> _serviceMock;
        private readonly ReportsController _controller;
        private readonly Guid _userId;

        public ReportsControllerTests()
        {
            _serviceMock = new Mock<IReportsService>();
            _controller = new ReportsController(_serviceMock.Object);

            _userId = Guid.NewGuid();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            }, "TestAuth"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        // ---------- DAILY REPORT ----------

        [Fact]
        public async Task GetDailyReport_ShouldReturnBadRequest_WhenDateIsDefault()
        {
            var result = await _controller.GetDailyReport(default);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date.", bad.Value);
        }

        [Fact]
        public async Task GetDailyReport_ShouldReturnOk_WhenValid()
        {
            var date = new DateTime(2025, 1, 10);

            var report = new FinancialReportDto
            {
                TotalIncome = 100,
                TotalExpenses = 50
            };

            _serviceMock
                .Setup(s => s.GetDailyReportAsync(_userId, date))
                .ReturnsAsync(report);

            var result = await _controller.GetDailyReport(date);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, ok.Value);
        }

        // ---------- PERIOD REPORT ----------

        [Fact]
        public async Task GetPeriodReport_ShouldReturnBadRequest_WhenDatesDefault()
        {
            var result = await _controller.GetPeriodReport(default, default);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date range.", bad.Value);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnBadRequest_WhenEndBeforeStart()
        {
            var start = new DateTime(2025, 1, 10);
            var end = new DateTime(2025, 1, 5);

            var result = await _controller.GetPeriodReport(start, end);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("End date must be after start date.", bad.Value);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnOk_WhenValidRange()
        {
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 31);

            var report = new FinancialReportDto
            {
                TotalIncome = 500,
                TotalExpenses = 300
            };

            _serviceMock
                .Setup(s => s.GetPeriodReportAsync(_userId, start, end))
                .ReturnsAsync(report);

            var result = await _controller.GetPeriodReport(start, end);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, ok.Value);
        }
    }
}
