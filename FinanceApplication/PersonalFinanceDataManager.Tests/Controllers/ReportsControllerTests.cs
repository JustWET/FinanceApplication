using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.DTOs;
using PersonalFinanceDataManager.Core.Services.Interfaces;

namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportsService> _reportsServiceMock;
        private readonly ReportsController _controller;

        public ReportsControllerTests()
        {
            _reportsServiceMock = new Mock<IReportsService>();
            _controller = new ReportsController(_reportsServiceMock.Object);
        }

        [Fact]
        public async Task GetDailyReport_ShouldReturnBadRequest_WhenDateIsDefault()
        {
            var result = await _controller.GetDailyReport(default);

            Assert.IsType<BadRequestObjectResult>(result);
            _reportsServiceMock.Verify(s => s.GetDailyReportAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GetDailyReport_ShouldReturnOk_WithReport()
        {
            var date = DateTime.Today;
            var report = new FinancialReportDto();

            _reportsServiceMock
                .Setup(s => s.GetDailyReportAsync(date))
                .ReturnsAsync(report);

            var result = await _controller.GetDailyReport(date);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, okResult.Value);
            _reportsServiceMock.Verify(s => s.GetDailyReportAsync(date), Times.Once);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnBadRequest_WhenStartDateInvalid()
        {
            var result = await _controller.GetPeriodReport(default, DateTime.Today);

            Assert.IsType<BadRequestObjectResult>(result);
            _reportsServiceMock.Verify(s => s.GetPeriodReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnBadRequest_WhenEndDateInvalid()
        {
            var result = await _controller.GetPeriodReport(DateTime.Today, default);

            Assert.IsType<BadRequestObjectResult>(result);
            _reportsServiceMock.Verify(s => s.GetPeriodReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnBadRequest_WhenEndDateBeforeStart()
        {
            var start = DateTime.Today;
            var end = start.AddDays(-1);

            var result = await _controller.GetPeriodReport(start, end);

            Assert.IsType<BadRequestObjectResult>(result);
            _reportsServiceMock.Verify(s => s.GetPeriodReportAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task GetPeriodReport_ShouldReturnOk_WithReport()
        {
            var start = DateTime.Today.AddDays(-7);
            var end = DateTime.Today;
            var report = new FinancialReportDto();

            _reportsServiceMock
                .Setup(s => s.GetPeriodReportAsync(start, end))
                .ReturnsAsync(report);

            var result = await _controller.GetPeriodReport(start, end);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(report, okResult.Value);

            _reportsServiceMock.Verify(s => s.GetPeriodReportAsync(start, end), Times.Once);
        }
    }
}
