using Moq;
using PersonalFinanceDataManager.Core.Services;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalFinanceDataManager.Tests.Services
{
    public class ReportsServiceTests
    {
        private readonly Mock<IFinancialOperationsRepository> _operationsRepoMock;
        private readonly Mock<IOperationTypesRepository> _typesRepoMock;
        private readonly ReportsService _service;

        public ReportsServiceTests()
        {
            _operationsRepoMock = new Mock<IFinancialOperationsRepository>();
            _typesRepoMock = new Mock<IOperationTypesRepository>();
            _service = new ReportsService(_operationsRepoMock.Object, _typesRepoMock.Object);
        }

        [Fact]
        public async Task GetDailyReportAsync_ShouldReturnCorrectTotals()
        {
            var userId = Guid.NewGuid();
            var date = DateTime.Today;
            var incomeTypeId = Guid.NewGuid();
            var expenseTypeId = Guid.NewGuid();

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = incomeTypeId, Amount = 100, Date = date.AddHours(10)},
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = expenseTypeId, Amount = 40, Date = date.AddHours(15)},
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = expenseTypeId, Amount = 999, Date = date.AddDays(1)},
            };

            var types = new List<OperationType>
            {
                new OperationType { Id = incomeTypeId, Name = "Salary", IsIncome = true },
                new OperationType { Id = expenseTypeId, Name = "Food", IsIncome = false }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(types);

            var report = await _service.GetDailyReportAsync(userId, date);

            Assert.Equal(100, report.TotalIncome);
            Assert.Equal(40, report.TotalExpenses);
            Assert.Equal(60, report.NetResult);
            Assert.Equal(2, report.Operations.Count());
        }

        [Fact]
        public async Task GetDailyReportAsync_ShouldIgnoreDeletedOperations()
        {
            var userId = Guid.NewGuid();
            var date = DateTime.Today;
            var incomeTypeId = Guid.NewGuid();
            var expenseTypeId = Guid.NewGuid();

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = incomeTypeId, Amount = 100, Date = date.AddHours(10)},
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = expenseTypeId, Amount = 40, Date = date.AddHours(15), IsDeleted = true},
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = expenseTypeId, Amount = 999, Date = date.AddDays(1)},
            };

            var types = new List<OperationType>
            {
                new OperationType { Id = incomeTypeId, Name = "Salary", IsIncome = true },
                new OperationType { Id = expenseTypeId, Name = "Food", IsIncome = false }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(types);

            var report = await _service.GetDailyReportAsync(userId, date);

            Assert.Equal(100, report.TotalIncome);
            Assert.Equal(0, report.TotalExpenses);
            Assert.Equal(100, report.NetResult);
            Assert.Equal(1, report.Operations.Count());
        }

        [Fact]
        public async Task GetPeriodReportAsync_ShouldFilterDatesCorrectly()
        {
            var userId = Guid.NewGuid();

            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);

            var typeId = Guid.NewGuid();

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = typeId, Amount = 30, Date = new DateTime(2024, 1, 10)},
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = typeId, Amount = 70, Date = new DateTime(2024, 2, 1)},
            };

            var types = new List<OperationType>
            {
                new OperationType { Id = typeId, Name = "Food", IsIncome = false }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(types);

            var report = await _service.GetPeriodReportAsync(userId, start, end);

            Assert.Single(report.Operations);
            Assert.Equal(30, report.TotalExpenses);
        }
    }

}
