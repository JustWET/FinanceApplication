using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceDataManager.Core.Abstractions;
using PersonalFinanceDataManager.Core.DTOs.OperationType;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OperationTypesController : MyControllerBase
    {
        private readonly IOperationTypesService _service;

        public OperationTypesController(IOperationTypesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var types = await _service.GetAllAsync(userId);
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();
            var type = await _service.GetByIdAsync(userId, id);

            if (type == null)
                return NotFound();

            return Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOperationTypeDto type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            var created = await _service.CreateAsync(userId, type);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOperationTypeDto inputType)
        {
            if (id != inputType.Id)
                return BadRequest("ID mismatch");

            var userId = GetUserId();

            var exists = await _service.GetByIdAsync(userId, id);
            if (exists == null)
                return NotFound();

            var updated = await _service.UpdateAsync(userId, inputType);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            var exists = await _service.GetByIdAsync(userId, id);
            if (exists == null)
                return NotFound();

            await _service.DeleteAsync(userId, id);
            return NoContent();
        }
    }
}
