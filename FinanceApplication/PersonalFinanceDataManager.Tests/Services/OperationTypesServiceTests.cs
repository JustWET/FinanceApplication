using Moq;
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

        //[Fact]
        //public async Task GetAllAsync_ShouldReturnListOfOperationTypes()
        //{
        //    var items = new List<OperationType>
        //    {
        //        new OperationType { Id = Guid.NewGuid(), Name = "Test" }
        //    };

        //    _typesRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

        //    var result = await _service.GetAllAsync();

        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //}

        //[Fact]
        //public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoItems()
        //{
        //    _typesRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<OperationType>());

        //    var result = await _service.GetAllAsync();

        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}

        //[Fact]
        //public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        //{
        //    var id = Guid.NewGuid();
        //    var item = new OperationType { Id = id, Name = "Test" };

        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(item);

        //    var result = await _service.GetByIdAsync(id);

        //    Assert.NotNull(result);
        //    Assert.Equal(id, result.Id);
        //}

        //[Fact]
        //public async Task GetByIdAsync_ShouldThrowException_WhenNotFound()
        //{
        //    var id = Guid.NewGuid();
        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((OperationType)null);

        //    await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(id));
        //}

        //[Fact]
        //public async Task CreateAsync_ShouldReturnCreatedEntity()
        //{
        //    var item = new OperationType { Id = Guid.NewGuid(), Name = "CreateTest" };

        //    _typesRepositoryMock.Setup(r => r.AddAsync(item)).Returns(Task.CompletedTask);

        //    var result = await _service.CreateAsync(item);

        //    Assert.Equal(item.Name, result.Name);
        //    _typesRepositoryMock.Verify(r => r.AddAsync(item), Times.Once);
        //}

        //[Fact]
        //public async Task CreateAsync_ShouldThrowException_WhenNameIsEmpty()
        //{
        //    var item = new OperationType { Id = Guid.NewGuid(), Name = "" };

        //    await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(item));
        //}

        //[Fact]
        //public async Task UpdateAsync_ShouldUpdateEntity_WhenExists()
        //{
        //    var id = Guid.NewGuid();
        //    var existing = new OperationType { Id = id, Name = "Old", Description = "OldDesc" };
        //    var updated = new OperationType { Id = id, Name = "New", Description = "NewDesc" };

        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        //    _typesRepositoryMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        //    var result = await _service.UpdateAsync(updated);

        //    Assert.Equal("New", result.Name);
        //    Assert.Equal("NewDesc", result.Description);
        //    _typesRepositoryMock.Verify(r => r.UpdateAsync(existing), Times.Once);
        //}

        //[Fact]
        //public async Task UpdateAsync_ShouldThrowException_WhenNotFound()
        //{
        //    var updated = new OperationType { Id = Guid.NewGuid(), Name = "New" };

        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(updated.Id)).ReturnsAsync((OperationType)null);

        //    await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(updated));
        //}

        //[Fact]
        //public async Task DeleteAsync_ShouldDeleteEntity_WhenExistsAndNoLinkedOperations()
        //{
        //    var id = Guid.NewGuid();
        //    var item = new OperationType { Id = id, Name = "ToDelete" };

        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(item);
        //    _operationsRepositoryMock.Setup(r => r.ExistsByTypeIdAsync(id)).ReturnsAsync(false);
        //    _typesRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        //    await _service.DeleteAsync(id);

        //    _typesRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        //    _operationsRepositoryMock.Verify(r => r.ExistsByTypeIdAsync(id), Times.Once);
        //}

        //[Fact]
        //public async Task DeleteAsync_ShouldThrowException_WhenTypeDoesNotExist()
        //{
        //    var id = Guid.NewGuid();
        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((OperationType)null);

        //    await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(id));

        //    _typesRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        //}

        //[Fact]
        //public async Task DeleteAsync_ShouldThrowException_WhenLinkedOperationsExist()
        //{
        //    var id = Guid.NewGuid();
        //    var item = new OperationType { Id = id, Name = "Blocked" };

        //    _typesRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(item);
        //    _operationsRepositoryMock.Setup(r => r.ExistsByTypeIdAsync(id)).ReturnsAsync(true);

        //    var ex = await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(id));

        //    Assert.Equal("Cannot delete operation type because it is used by existing financial operations.", ex.Message);

        //    _typesRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        //}
    }
}
