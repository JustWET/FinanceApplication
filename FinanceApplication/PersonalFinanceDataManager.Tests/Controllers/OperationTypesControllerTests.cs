using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.DTOs.OperationType;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using System.Security.Claims;


namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class OperationTypesControllerTests
    {
        private readonly Mock<IOperationTypesService> _serviceMock;
        private readonly OperationTypesController _controller;
        private readonly Guid _userId;

        public OperationTypesControllerTests()
        {
            _serviceMock = new Mock<IOperationTypesService>();
            _controller = new OperationTypesController(_serviceMock.Object);

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

        // ---------- GET ALL ----------

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithData()
        {
            var list = new List<OperationTypeDto>
            {
                new OperationTypeDto { Id = Guid.NewGuid(), Name = "Food" }
            };

            _serviceMock.Setup(s => s.GetAllAsync(_userId)).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(list, ok.Value);
        }

        // ---------- GET BY ID ----------

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var dto = new OperationTypeDto { Id = id, Name = "Food" };

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id)).ReturnsAsync(dto);

            var result = await _controller.GetById(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenServiceThrows()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Operation type not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.GetById(id));
        }

        // ---------- CREATE ----------

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var input = new CreateOperationTypeDto { Name = "Food" };

            var created = new OperationTypeDto
            {
                Id = Guid.NewGuid(),
                Name = "Food"
            };

            _serviceMock.Setup(s => s.CreateAsync(_userId, input)).ReturnsAsync(created);

            var result = await _controller.Create(input);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(OperationTypesController.GetById), createdResult.ActionName);
            Assert.Equal(created, createdResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(new CreateOperationTypeDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // ---------- UPDATE ----------

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var input = new UpdateOperationTypeDto { Id = Guid.NewGuid() };

            var result = await _controller.Update(Guid.NewGuid(), input);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", bad.Value);
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            var input = new UpdateOperationTypeDto { Id = id };

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Operation type not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.Update(id, input));
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccess()
        {
            var id = Guid.NewGuid();
            var input = new UpdateOperationTypeDto { Id = id, Name = "New" };

            var updated = new OperationTypeDto { Id = id, Name = "New" };

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id)).ReturnsAsync(updated);
            _serviceMock.Setup(s => s.UpdateAsync(_userId, input)).ReturnsAsync(updated);

            var result = await _controller.Update(id, input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updated, ok.Value);
        }

        // ---------- DELETE ----------

        [Fact]
        public async Task Delete_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Operation type not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(id));
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ReturnsAsync(new OperationTypeDto { Id = id });

            _serviceMock.Setup(s => s.DeleteAsync(_userId, id))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(id);

            Assert.IsType<NoContentResult>(result);
        }

        // ---------- TYPE USAGE ----------

        [Fact]
        public async Task GetTypeUsage_ShouldReturnOk_WithData()
        {
            var list = new List<OperationTypeUsageDto>
            {
                new OperationTypeUsageDto { OperationTypeId = Guid.NewGuid(), Count = 3 }
            };

            _serviceMock.Setup(s => s.GetOperationTypeUsageAsync(_userId)).ReturnsAsync(list);

            var result = await _controller.GetTypeUsage();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, ok.Value);
        }
    }

}
