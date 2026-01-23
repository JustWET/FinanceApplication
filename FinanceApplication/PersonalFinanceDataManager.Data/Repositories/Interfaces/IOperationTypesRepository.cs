using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Data.Repositories.Interfaces
{
    public interface IOperationTypesRepository
    {
        Task<List<OperationType>> GetAllAsync(Guid userId);
        Task<OperationType?> GetByIdAsync(Guid userId, Guid id);
        Task AddAsync(OperationType type);
        Task UpdateAsync(OperationType type);
        Task DeleteAsync(Guid userId, Guid id);
        Task<bool> ExistsWithNameAsync(Guid userId, string name, Guid? excludeId = null);
    }
}
