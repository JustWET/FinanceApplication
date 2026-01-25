using Moq;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Domain.Entities;
using PersonalFinanceDataManager.Core.Controllers;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PersonalFinanceDataManager.Tests.Controllers
{
    public class FinancialOperationsControllerTests
    {
        private readonly Mock<IFinancialOperationsService> _serviceMock;
        private readonly FinancialOperationsController _controller;
        private readonly Guid _userId;

        public FinancialOperationsControllerTests()
        {
            _serviceMock = new Mock<IFinancialOperationsService>();
            _controller = new FinancialOperationsController(_serviceMock.Object);

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
        public async Task GetAll_ShouldReturnOk_WithDtos()
        {
            var list = new List<FinancialOperationDto>
            {
                new FinancialOperationDto { Id = Guid.NewGuid(), OperationTypeId = Guid.NewGuid(), Amount = 100,
                    Date = DateTime.Now, OperationTypeName = "Name", Description = "Desc", IsIncome = true}
            };

            _serviceMock.Setup(s => s.GetAllDtosAsync(_userId)).ReturnsAsync(list);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(list, ok.Value);
        }

        // ---------- GET BY ID ----------

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var dto = new FinancialOperationDto
            {
                Id = Guid.NewGuid(),
                OperationTypeId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now,
                OperationTypeName = "Name",
                Description = "Desc",
                IsIncome = true
            };

            _serviceMock.Setup(s => s.GetDtoByIdAsync(_userId, id)).ReturnsAsync(dto);

            var result = await _controller.GetById(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task GetById_ShouldThrow_WhenServiceThrows()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetDtoByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Financial operation not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.GetById(id));
        }

        // ---------- CREATE ----------

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var input = new CreateFinancialOperationDto
            {
                Amount = 100,
                Date = DateTime.Now,
                OperationTypeId = Guid.NewGuid()
            };

            var created = new FinancialOperationDto
            {
                Id = Guid.NewGuid(),
                OperationTypeId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now,
                OperationTypeName = "Name",
                Description = "Desc",
                IsIncome = true
            };

            _serviceMock.Setup(s => s.CreateAsync(_userId, input)).ReturnsAsync(created);

            var result = await _controller.Create(input);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(FinancialOperationsController.GetById), createdResult.ActionName);
            Assert.Equal(created, createdResult.Value);
        }

        // ---------- UPDATE ----------

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var input = new UpdateFinancialOperationDto
            {
                Id = Guid.NewGuid(), 
                OperationTypeId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now,
            };

            var result = await _controller.Update(Guid.NewGuid(), input);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", bad.Value);
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            var input = new UpdateFinancialOperationDto 
            { 
                Id = id,
                OperationTypeId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now,
            };

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Financial operation not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.Update(id, input));
        }

        [Fact]
        public async Task Update_ShouldReturnOk_WhenSuccess()
        {
            var id = Guid.NewGuid();

            var input = new UpdateFinancialOperationDto
            {
                Id = id,
                Amount = 200,
                Date = DateTime.Now,
                OperationTypeId = Guid.NewGuid()
            };

            var entity = new FinancialOperation { Id = id };

            var updatedDto = new FinancialOperationDto
            {
                Id = Guid.NewGuid(),
                OperationTypeId = Guid.NewGuid(),
                Amount = 100,
                Date = DateTime.Now,
                OperationTypeName = "Name",
                Description = "Desc",
                IsIncome = true
            };

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id)).ReturnsAsync(entity);
            _serviceMock.Setup(s => s.UpdateAsync(_userId, input)).ReturnsAsync(updatedDto);

            var result = await _controller.Update(id, input);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedDto, ok.Value);
        }

        // ---------- DELETE ----------

        [Fact]
        public async Task Delete_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ThrowsAsync(new Exception("Financial operation not found."));

            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(id));
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccess()
        {
            var id = Guid.NewGuid();

            _serviceMock.Setup(s => s.GetByIdAsync(_userId, id))
                .ReturnsAsync(new FinancialOperation { Id = id });

            _serviceMock.Setup(s => s.DeleteAsync(_userId, id))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(id);

            Assert.IsType<NoContentResult>(result);
        }
    }

}
