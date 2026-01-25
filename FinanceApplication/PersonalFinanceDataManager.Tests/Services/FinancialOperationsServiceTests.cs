using Moq;
using PersonalFinanceDataManager.Core.Services;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using System;

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

        // ---------- GetAllAsync ----------

        [Fact]
        public async Task GetAllAsync_ShouldReturnRepositoryResult()
        {
            var userId = Guid.NewGuid();
            var list = new List<FinancialOperation>();

            _operationsRepositoryMock
                .Setup(r => r.GetAllAsync(userId))
                .ReturnsAsync(list);

            var result = await _service.GetAllAsync(userId);

            Assert.Equal(list, result);
        }

        // ---------- GetAllDtosAsync ----------

        [Fact]
        public async Task GetAllDtosAsync_ShouldMapOperationsWithTypes()
        {
            var userId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { Id = Guid.NewGuid(), UserId = userId, TypeId = typeId, Amount = 10 }
            };

                var types = new List<OperationType>
            {
                new OperationType { Id = typeId, Name = "Food", IsIncome = false }
            };

            _operationsRepositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(operations);
            _typesRepositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(types);

            var result = await _service.GetAllDtosAsync(userId);

            Assert.Single(result);
            Assert.Equal("Food", result[0].OperationTypeName);
            Assert.Equal(10, result[0].Amount);
        }

        // ---------- GetByIdAsync ----------

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var op = new FinancialOperation { Id = id };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync(op);

            var result = await _service.GetByIdAsync(userId, id);

            Assert.Equal(op, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync((FinancialOperation?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetByIdAsync(userId, id));
        }

        // ---------- GetDtoByIdAsync ----------

        [Fact]
        public async Task GetDtoByIdAsync_ShouldReturnDto_WhenOperationAndTypeExist()
        {
            var userId = Guid.NewGuid();
            var opId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var op = new FinancialOperation { Id = opId, TypeId = typeId, Amount = 5 };
            var type = new OperationType { Id = typeId, Name = "Salary", IsIncome = true };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, opId)).ReturnsAsync(op);
            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, typeId)).ReturnsAsync(type);

            var result = await _service.GetDtoByIdAsync(userId, opId);

            Assert.Equal("Salary", result.OperationTypeName);
            Assert.True(result.IsIncome);
        }

        [Fact]
        public async Task GetDtoByIdAsync_ShouldThrow_WhenOperationNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync((FinancialOperation?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetDtoByIdAsync(userId, id));
        }

        [Fact]
        public async Task GetDtoByIdAsync_ShouldThrow_WhenTypeNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var op = new FinancialOperation { Id = id, TypeId = typeId };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync(op);
            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, typeId)).ReturnsAsync((OperationType?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetDtoByIdAsync(userId, id));
        }

        // ---------- CreateAsync ----------

        [Fact]
        public async Task CreateAsync_ShouldCreateOperation_WhenValid()
        {
            var userId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var dto = new CreateFinancialOperationDto
            {
                Amount = 100,
                OperationTypeId = typeId,
                Date = DateTime.Now
            };

            var type = new OperationType { Id = typeId, Name = "Food" };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, typeId)).ReturnsAsync(type);

            var result = await _service.CreateAsync(userId, dto);

            _operationsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<FinancialOperation>()), Times.Once);
            Assert.Equal(100, result.Amount);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenAmountInvalid()
        {
            var userId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var dto = new CreateFinancialOperationDto
            {
                Amount = 0,
                OperationTypeId = typeId,
                Date = DateTime.Now
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateAsync(userId, dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenTypeNotExists()
        {
            var userId = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var dto = new CreateFinancialOperationDto
            {
                Amount = 100,
                OperationTypeId = typeId,
                Date = DateTime.Now
            };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, typeId)).ReturnsAsync((OperationType?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateAsync(userId, dto));
        }

        // ---------- UpdateAsync ----------

        [Fact]
        public async Task UpdateAsync_ShouldUpdate_WhenValid()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var typeId = Guid.NewGuid();

            var existing = new FinancialOperation { Id = id, UserId = userId };

            var dto = new UpdateFinancialOperationDto
            {
                Id = id,
                Amount = 50,
                OperationTypeId = typeId,
                Date = DateTime.Today
            };

            var type = new OperationType { Id = typeId, Name = "Food" };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync(existing);
            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, typeId)).ReturnsAsync(type);

            var result = await _service.UpdateAsync(userId, dto);

            _operationsRepositoryMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            Assert.Equal(50, result.Amount);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNotFound()
        {
            var userId = Guid.NewGuid();

            var dto = new UpdateFinancialOperationDto
            {
                Id = Guid.NewGuid(),
                Amount = 50,
                OperationTypeId = Guid.NewGuid(),
                Date = DateTime.Today
            };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, dto.Id)).ReturnsAsync((FinancialOperation?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(userId, dto));
        }

        // ---------- DeleteAsync ----------

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryDelete()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var op = new FinancialOperation { Id = id };

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync(op);

            await _service.DeleteAsync(userId, id);

            _operationsRepositoryMock.Verify(r => r.DeleteAsync(userId, id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            _operationsRepositoryMock.Setup(r => r.GetByIdAsync(userId, id)).ReturnsAsync((FinancialOperation?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.DeleteAsync(userId, id));
        }
    }

}
