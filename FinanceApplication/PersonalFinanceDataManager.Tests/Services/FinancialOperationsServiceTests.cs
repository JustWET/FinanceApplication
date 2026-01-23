using Moq;
using PersonalFinanceDataManager.Core.Services;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;

namespace PersonalFinanceDataManager.Tests.Services
{


    public class FinancialOperationsServiceTests
    {
        private readonly Mock<IFinancialOperationsRepository> _operationsRepositoryMock;
        private readonly Mock<IOperationTypesRepository> _typesRepositoryMock;
        private readonly FinancialOperationsService _service;

        public FinancialOperationsServiceTests()
        {
            _operationsRepositoryMock = new Mock<IFinancialOperationsRepository>();
            _typesRepositoryMock = new Mock<IOperationTypesRepository>();
            _service = new FinancialOperationsService(_operationsRepositoryMock.Object, _typesRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_ListOfOperations()
        {
            var ops = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), Amount = 100 }
            };

            _operationsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(ops);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOperation_WhenExists()
        {
            var id = Guid.NewGuid();
            var op = new FinancialOperation { Id = id };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(op);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((FinancialOperation)null);

            await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(id));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAmountInvalid()
        {
            var op = new FinancialOperation { Amount = 0 };

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(op));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenOperationTypeDoesNotExist()
        {
            var op = new FinancialOperation { Amount = 100, TypeId = Guid.NewGuid() };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(op.TypeId)).ReturnsAsync((OperationType)null);

            await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(op));
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAdd_WhenValid()
        {
            var type = new OperationType { Id = Guid.NewGuid() };
            var op = new FinancialOperation { Amount = 100, TypeId = type.Id };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(type.Id)).ReturnsAsync(type);

            var result = await _service.CreateAsync(op);

            _operationsRepositoryMock.Verify(r => r.AddAsync(op), Times.Once);
            Assert.Equal(op, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            var op = new FinancialOperation { Id = Guid.NewGuid() };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(op.Id)).ReturnsAsync((FinancialOperation)null);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(op));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenAmountInvalid()
        {
            var op = new FinancialOperation { Id = Guid.NewGuid(), Amount = 0 };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(op.Id)).ReturnsAsync(new FinancialOperation());

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(op));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenOperationTypeDoesNotExist()
        {
            var id = Guid.NewGuid();
            var op = new FinancialOperation { Id = id, Amount = 10, TypeId = Guid.NewGuid() };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new FinancialOperation());
            _typesRepositoryMock.Setup(r => r.GetByIdAsync(op.TypeId)).ReturnsAsync((OperationType)null);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(op));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((FinancialOperation)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDelete_WhenFound()
        {
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new FinancialOperation());

            await _service.DeleteAsync(id);

            _operationsRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }

}
