using Moq;
using PersonalFinanceDataManager.Core.Services;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;

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
            var date = DateTime.Today;
            var typeIncome = new OperationType { Id = Guid.NewGuid(), Name = "Salary", IsIncome = true, Description = "Monthly income" };
            var typeExpense = new OperationType { Id = Guid.NewGuid(), Name = "Food", IsIncome = false, Description = "Groceries" };

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 100, Date = date, TypeId = typeIncome.Id },
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 50, Date = date, TypeId = typeExpense.Id }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<OperationType> { typeIncome, typeExpense });

            var result = await _service.GetDailyReportAsync(date);

            Assert.Equal(100, result.TotalIncome);
            Assert.Equal(50, result.TotalExpenses);
            Assert.Equal(2, result.Operations.Count);
        }

        [Fact]
        public async Task GetDailyReportAsync_ShouldIgnoreDeletedOperations()
        {
            var date = DateTime.Today;
            var type = new OperationType { Id = Guid.NewGuid(), Name = "Any", IsIncome = true, Description = "" };

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 100, Date = date, TypeId = type.Id, IsDeleted = true }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<OperationType> { type });

            var result = await _service.GetDailyReportAsync(date);

            Assert.Empty(result.Operations);
            Assert.Equal(0, result.TotalIncome);
            Assert.Equal(0, result.TotalExpenses);
        }

        [Fact]
        public async Task GetPeriodReportAsync_ShouldFilterDatesCorrectly()
        {
            var start = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 1, 31);

            var type = new OperationType { Id = Guid.NewGuid(), Name = "Any", IsIncome = false, Description = "" };

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 20, Date = new DateTime(2024, 1, 10), TypeId = type.Id },
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 30, Date = new DateTime(2024, 2, 1), TypeId = type.Id }
            };

            _operationsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(operations);
            _typesRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<OperationType> { type });

            var result = await _service.GetPeriodReportAsync(start, end);

            Assert.Single(result.Operations);
            Assert.Equal(20, result.TotalExpenses);
        }
    }

}
