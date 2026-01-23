using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;


namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class OperationTypesControllerTests
    {
        private readonly Mock<IOperationTypesService> _serviceMock;
        private readonly OperationTypesController _controller;

        public OperationTypesControllerTests()
        {
            _serviceMock = new Mock<IOperationTypesService>();
            _controller = new OperationTypesController(_serviceMock.Object);
        }

        //[Fact]
        //public async Task GetAll_ShouldReturnOk_WithData()
        //{
        //    var list = new List<OperationType> { new OperationType { Id = Guid.NewGuid(), Name = "Test" } };
        //    _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        //    var result = await _controller.GetAll();

        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(list, okResult.Value);
        //}

        //[Fact]
        //public async Task GetById_ShouldReturnOk_WhenExists()
        //{
        //    var id = Guid.NewGuid();
        //    var type = new OperationType { Id = id, Name = "Test" };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(type);

        //    var result = await _controller.GetById(id);

        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(type, ok.Value);
        //}

        //[Fact]
        //public async Task GetById_ShouldReturnNotFound_WhenMissing()
        //{
        //    var id = Guid.NewGuid();
        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((OperationType)null);

        //    var result = await _controller.GetById(id);

        //    Assert.IsType<NotFoundResult>(result);
        //}

        //[Fact]
        //public async Task Create_ShouldReturnCreated_WhenValidModel()
        //{
        //    var newType = new OperationType { Id = Guid.NewGuid(), Name = "Test" };

        //    _serviceMock.Setup(s => s.CreateAsync(newType)).ReturnsAsync(newType);

        //    var result = await _controller.Create(newType);

        //    var created = Assert.IsType<CreatedAtActionResult>(result);
        //    Assert.Equal(nameof(OperationTypesController.GetById), created.ActionName);
        //    Assert.Equal(newType, created.Value);
        //}

        //[Fact]
        //public async Task Create_ShouldReturnBadRequest_WhenModelInvalid()
        //{
        //    var newType = new OperationType { Id = Guid.NewGuid() };
        //    _controller.ModelState.AddModelError("Name", "Required");

        //    var result = await _controller.Create(newType);

        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        //{
        //    var id = Guid.NewGuid();
        //    var type = new OperationType { Id = Guid.NewGuid() };

        //    var result = await _controller.Update(id, type);

        //    var bad = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("ID mismatch", bad.Value);
        //}

        //[Fact]
        //public async Task Update_ShouldReturnNotFound_WhenDoesNotExist()
        //{
        //    var id = Guid.NewGuid();
        //    var type = new OperationType { Id = id, Name = "Test" };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((OperationType)null);

        //    var result = await _controller.Update(id, type);

        //    Assert.IsType<NotFoundResult>(result);
        //}

        //[Fact]
        //public async Task Update_ShouldReturnOk_WhenUpdated()
        //{
        //    var id = Guid.NewGuid();
        //    var type = new OperationType { Id = id, Name = "Test" };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(type);
        //    _serviceMock.Setup(s => s.UpdateAsync(type)).ReturnsAsync(type);

        //    var result = await _controller.Update(id, type);

        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(type, ok.Value);
        //}

        //[Fact]
        //public async Task Delete_ShouldReturnNotFound_WhenMissing()
        //{
        //    var id = Guid.NewGuid();
        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((OperationType)null);

        //    var result = await _controller.Delete(id);

        //    Assert.IsType<NotFoundResult>(result);
        //}

        //[Fact]
        //public async Task Delete_ShouldReturnNoContent_WhenDeleted()
        //{
        //    var id = Guid.NewGuid();
        //    var type = new OperationType { Id = id };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(type);

        //    var result = await _controller.Delete(id);

        //    _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        //    Assert.IsType<NoContentResult>(result);
        //}
    }

}
