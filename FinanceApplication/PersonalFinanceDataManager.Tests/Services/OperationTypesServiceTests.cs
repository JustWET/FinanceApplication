using Moq;
using PersonalFinanceDataManager.Core.DTOs.OperationType;
using PersonalFinanceDataManager.Core.Services;
using PersonalFinanceDataManager.Data.Repositories;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Tests.Services
{
    public class OperationTypesServiceTests
    {
        private readonly Mock<IFinancialOperationsRepository> _operationsRepositoryMock;
        private readonly Mock<IOperationTypesRepository> _typesRepositoryMock;
        private readonly OperationTypesService _service;

        public OperationTypesServiceTests()
        {
            _operationsRepositoryMock = new Mock<IFinancialOperationsRepository>();
            _typesRepositoryMock = new Mock<IOperationTypesRepository>();
            _service = new OperationTypesService(_operationsRepositoryMock.Object, _typesRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var userId = Guid.NewGuid();

            var entities = new List<OperationType>
            {
                new OperationType { Id = Guid.NewGuid(), UserId = userId, Name = "A", IsIncome = true, Description = "D1" },
                new OperationType { Id = Guid.NewGuid(), UserId = userId, Name = "B", IsIncome = false, Description = "D2" }
            };

            _typesRepositoryMock.Setup(r => r.GetAllAsync(userId))
                .ReturnsAsync(entities);

            var result = await _service.GetAllAsync(userId);

            Assert.Equal(2, result.Count);
            Assert.Equal(entities[0].Id, result[0].Id);
            Assert.Equal(entities[0].Name, result[0].Name);
            Assert.Equal(entities[1].IsIncome, result[1].IsIncome);
        }

        // ---------- GetByIdAsync ----------

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var entity = new OperationType
            {
                Id = id,
                UserId = userId,
                Name = "Test",
                IsIncome = true,
                Description = "Desc"
            };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync(entity);

            var result = await _service.GetByIdAsync(userId, id);

            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_Throws_WhenNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync((OperationType?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.GetByIdAsync(userId, id));
        }

        // ---------- CreateAsync ----------

        [Fact]
        public async Task CreateAsync_Throws_WhenNameEmpty()
        {
            var userId = Guid.NewGuid();

            var dto = new CreateOperationTypeDto
            {
                Name = "   ",
                IsIncome = true
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateAsync(userId, dto));
        }

        [Fact]
        public async Task CreateAsync_Throws_WhenNameExists()
        {
            var userId = Guid.NewGuid();

            _typesRepositoryMock.Setup(r => r.ExistsWithNameAsync(userId, "Food", null))
                .ReturnsAsync(true);

            var dto = new CreateOperationTypeDto
            {
                Name = "Food",
                IsIncome = false
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateAsync(userId, dto));
        }

        [Fact]
        public async Task CreateAsync_CreatesAndReturnsDto()
        {
            var userId = Guid.NewGuid();

            _typesRepositoryMock.Setup(r => r.ExistsWithNameAsync(userId, "Salary", null))
                .ReturnsAsync(false);

            OperationType? savedEntity = null;

            _typesRepositoryMock.Setup(r => r.AddAsync(It.IsAny<OperationType>()))
                .Callback<OperationType>(e => savedEntity = e)
                .Returns(Task.CompletedTask);

            var dto = new CreateOperationTypeDto
            {
                Name = "Salary",
                IsIncome = true,
                Description = "Monthly"
            };

            var result = await _service.CreateAsync(userId, dto);

            Assert.NotNull(savedEntity);
            Assert.Equal(userId, savedEntity!.UserId);
            Assert.Equal(dto.Name, result.Name);

            _typesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<OperationType>()), Times.Once);
        }

        // ---------- UpdateAsync ----------

        [Fact]
        public async Task UpdateAsync_Throws_WhenNotFound()
        {
            var userId = Guid.NewGuid();

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<Guid>()))
                .ReturnsAsync((OperationType?)null);

            var dto = new UpdateOperationTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(userId, dto));
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenNameExists()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var entity = new OperationType { Id = id, UserId = userId };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync(entity);

            _typesRepositoryMock.Setup(r => r.ExistsWithNameAsync(userId, "Food", id))
                .ReturnsAsync(true);

            var dto = new UpdateOperationTypeDto
            {
                Id = id,
                Name = "Food"
            };

            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(userId, dto));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntity()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var entity = new OperationType
            {
                Id = id,
                UserId = userId,
                Name = "Old",
                IsIncome = false
            };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync(entity);

            _typesRepositoryMock.Setup(r => r.ExistsWithNameAsync(userId, "New", id))
                .ReturnsAsync(false);

            var dto = new UpdateOperationTypeDto
            {
                Id = id,
                Name = "New",
                IsIncome = true,
                Description = "Updated"
            };

            var result = await _service.UpdateAsync(userId, dto);

            Assert.Equal("New", entity.Name);
            Assert.True(entity.IsIncome);
            Assert.Equal("New", result.Name);

            _typesRepositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
        }

        // ---------- DeleteAsync ----------

        [Fact]
        public async Task DeleteAsync_Throws_WhenNotFound()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync((OperationType?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.DeleteAsync(userId, id));
        }

        [Fact]
        public async Task DeleteAsync_Throws_WhenLinkedOperationsExist()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var entity = new OperationType { Id = id, UserId = userId };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync(entity);

            _operationsRepositoryMock.Setup(r => r.ExistsByTypeIdAsync(userId, id))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() =>
                _service.DeleteAsync(userId, id));
        }

        [Fact]
        public async Task DeleteAsync_Deletes_WhenValid()
        {
            var userId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var entity = new OperationType { Id = id, UserId = userId };

            _typesRepositoryMock.Setup(r => r.GetByIdAsync(userId, id))
                .ReturnsAsync(entity);

            _operationsRepositoryMock.Setup(r => r.ExistsByTypeIdAsync(userId, id))
                .ReturnsAsync(false);

            await _service.DeleteAsync(userId, id);

            _typesRepositoryMock.Verify(r => r.DeleteAsync(userId, id), Times.Once);
        }

        // ---------- GetOperationTypeUsageAsync ----------

        [Fact]
        public async Task GetOperationTypeUsageAsync_GroupsAndCounts()
        {
            var userId = Guid.NewGuid();
            var type1 = Guid.NewGuid();
            var type2 = Guid.NewGuid();

            var operations = new List<FinancialOperation>
            {
                new FinancialOperation { UserId = userId, TypeId = type1 },
                new FinancialOperation { UserId = userId, TypeId = type1 },
                new FinancialOperation { UserId = userId, TypeId = type2 }
            };

            _operationsRepositoryMock.Setup(r => r.GetAllAsync(userId))
                .ReturnsAsync(operations);

            var result = await _service.GetOperationTypeUsageAsync(userId);

            Assert.Equal(2, result.Count);

            var t1 = result.First(x => x.OperationTypeId == type1);
            var t2 = result.First(x => x.OperationTypeId == type2);

            Assert.Equal(2, t1.Count);
            Assert.Equal(1, t2.Count);
        }
    }
}
