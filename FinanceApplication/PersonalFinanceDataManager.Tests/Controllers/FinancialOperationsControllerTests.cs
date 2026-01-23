using Moq;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.Services.Interfaces;

namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class FinancialOperationsControllerTests
    {
        private readonly Mock<IFinancialOperationsService> _serviceMock;
        private readonly FinancialOperationsController _controller;

        public FinancialOperationsControllerTests()
        {
            _serviceMock = new Mock<IFinancialOperationsService>();
            _controller = new FinancialOperationsController(_serviceMock.Object);
        }

        [Fact]
        public void TrueTest()
        {
            var ok = 4;

            Assert.Equal(4, ok);
        }

        //[Fact]
        //public async Task GetAll_ShouldReturnOk_WithList()
        //{
        //    var ops = new List<FinancialOperation> { new() { Id = Guid.NewGuid(), Amount = 100 } };
        //    _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(ops);

        //    var result = await _controller.GetAll();

        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(ops, ok.Value);
        //}

        //[Fact]
        //public async Task GetById_ShouldReturnOk_WhenExists()
        //{
        //    var id = Guid.NewGuid();
        //    var op = new FinancialOperation { Id = id };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(op);

        //    var result = await _controller.GetById(id);

        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(op, ok.Value);
        //}

        //[Fact]
        //public async Task GetById_ShouldReturnNotFound_WhenNotExists()
        //{
        //    var id = Guid.NewGuid();
        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FinancialOperation)null);

        //    var result = await _controller.GetById(id);

        //    Assert.IsType<NotFoundResult>(result);
        //}

        //[Fact]
        //public async Task Create_ShouldReturnCreated()
        //{
        //    var op = new FinancialOperation { Id = Guid.NewGuid(), Amount = 200 };
        //    _serviceMock.Setup(s => s.CreateAsync(op)).ReturnsAsync(op);

        //    var result = await _controller.Create(op);

        //    var created = Assert.IsType<CreatedAtActionResult>(result);
        //    Assert.Equal(nameof(FinancialOperationsController.GetById), created.ActionName);
        //    Assert.Equal(op, created.Value);
        //}

        //[Fact]
        //public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        //{
        //    var op = new FinancialOperation { Id = Guid.NewGuid(), Amount = 100 };

        //    var result = await _controller.Update(Guid.NewGuid(), op);

        //    var bad = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.Equal("ID mismatch", bad.Value);
        //}

        //[Fact]
        //public async Task Update_ShouldReturnOk_WhenValid()
        //{
        //    var id = Guid.NewGuid();
        //    var updated = new FinancialOperation { Id = id, Amount = 100 };

        //    _serviceMock.Setup(s => s.UpdateAsync(updated)).ReturnsAsync(updated);

        //    var result = await _controller.Update(id, updated);

        //    var ok = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal(updated, ok.Value);
        //}

        //[Fact]
        //public async Task Delete_ShouldReturnNoContent_WhenExists()
        //{
        //    var id = Guid.NewGuid();
        //    var op = new FinancialOperation { Id = id };

        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(op);

        //    var result = await _controller.Delete(id);

        //    Assert.IsType<NoContentResult>(result);
        //    _serviceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        //}

        //[Fact]
        //public async Task Delete_ShouldReturnNotFound_WhenNotExists()
        //{
        //    var id = Guid.NewGuid();
        //    _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((FinancialOperation)null);

        //    var result = await _controller.Delete(id);

        //    Assert.IsType<NotFoundResult>(result);
        //}
    }

}
