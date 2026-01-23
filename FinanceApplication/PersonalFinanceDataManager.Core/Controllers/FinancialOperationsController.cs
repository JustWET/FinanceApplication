using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Core.Abstractions;
using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using PersonalFinanceDataManager.Core.Services.Interfaces;

namespace PersonalFinanceDataManager.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialOperationsController : MyControllerBase
    {
        private readonly IFinancialOperationsService _operationsService;

        public FinancialOperationsController(IFinancialOperationsService operationsService)
        {
            _operationsService = operationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var operations = await _operationsService.GetAllDtosAsync(userId);
            return Ok(operations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();
            var op = await _operationsService.GetDtoByIdAsync(userId, id);

            if (op == null)
                return NotFound();

            return Ok(op);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFinancialOperationDto operation)
        {
            var userId = GetUserId();
            
            var created = await _operationsService.CreateAsync(userId, operation);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateFinancialOperationDto operation)
        {
            if (id != operation.Id)
                return BadRequest("ID mismatch");

            var userId = GetUserId();

            var existing = await _operationsService.GetByIdAsync(userId, id);
            if (existing == null)
                return NotFound();

            var updated = await _operationsService.UpdateAsync(userId, operation);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            var exists = await _operationsService.GetByIdAsync(userId, id);
            if (exists == null)
                return NotFound();

            await _operationsService.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}
