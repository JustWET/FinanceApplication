using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services.Interfaces
{
    public interface IFinancialOperationsService
    {
        Task<List<FinancialOperation>> GetAllAsync(Guid userId);
        Task<List<FinancialOperationDto>> GetAllDtosAsync(Guid userId);
        Task<FinancialOperation> GetByIdAsync(Guid userId, Guid id);
        Task<FinancialOperationDto> GetDtoByIdAsync(Guid userId, Guid operationId);
        Task<FinancialOperationDto> CreateAsync(Guid userId, CreateFinancialOperationDto opDto);
        Task<FinancialOperationDto> UpdateAsync(Guid userId, UpdateFinancialOperationDto opDto);
        Task DeleteAsync(Guid userId, Guid id);
    }
}
