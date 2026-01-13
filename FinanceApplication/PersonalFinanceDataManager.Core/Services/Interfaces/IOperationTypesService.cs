using PersonalFinanceDataManager.Core.DTOs.OperationType;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services.Interfaces
{
    public interface IOperationTypesService
    {
        Task<List<OperationTypeDto>> GetAllAsync(Guid userId);
        Task<OperationTypeDto> GetByIdAsync(Guid userId, Guid id);
        Task<OperationTypeDto> CreateAsync(Guid userId, CreateOperationTypeDto typeDto);
        Task<OperationTypeDto> UpdateAsync(Guid userId, UpdateOperationTypeDto typeDto);
        Task DeleteAsync(Guid userId, Guid id);
    }
}
