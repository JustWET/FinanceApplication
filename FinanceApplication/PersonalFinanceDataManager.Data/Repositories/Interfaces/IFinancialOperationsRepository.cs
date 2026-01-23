using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Data.Repositories.Interfaces
{
    public interface IFinancialOperationsRepository
    {
        Task<List<FinancialOperation>> GetAllAsync(Guid userId);
        Task<FinancialOperation?> GetByIdAsync(Guid userId, Guid id);
        Task AddAsync(FinancialOperation operation);
        Task UpdateAsync(FinancialOperation operation);
        Task<bool> ExistsByTypeIdAsync(Guid userId, Guid typeId);
        Task DeleteAsync(Guid userId, Guid id);
    }
}
