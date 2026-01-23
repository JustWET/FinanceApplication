using Microsoft.EntityFrameworkCore;
using PersonalFinanceDataManager.Data.Contexts;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Data.Repositories
{
    public class FinancialOperationsRepository : IFinancialOperationsRepository
    {
        private readonly AppDbContext _context;

        public FinancialOperationsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinancialOperation>> GetAllAsync(Guid userId)
        {
            return await _context.FinancialOperations
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<FinancialOperation?> GetByIdAsync(Guid userId, Guid id)
        {
            return await _context.FinancialOperations
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.Id == id && 
                    !o.IsDeleted);
        }

        public async Task AddAsync(FinancialOperation operation)
        {
            await _context.FinancialOperations.AddAsync(operation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialOperation operation)
        {
            _context.FinancialOperations.Update(operation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByTypeIdAsync(Guid userId, Guid typeId)
        {
            return await _context.FinancialOperations.AnyAsync(o => 
                o.UserId == userId &&
                o.TypeId == typeId &&
                !o.IsDeleted);
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var operation = await _context.FinancialOperations
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.Id == id);

            if (operation != null)
            {
                operation.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
