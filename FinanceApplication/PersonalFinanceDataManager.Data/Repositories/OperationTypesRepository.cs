using Microsoft.EntityFrameworkCore;
using PersonalFinanceDataManager.Data.Contexts;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Data.Repositories
{
    public class OperationTypesRepository : IOperationTypesRepository
    {
        private readonly AppDbContext _context;

        public OperationTypesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OperationType>> GetAllAsync(Guid userId)
        {
            return await _context.OperationTypes
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<OperationType?> GetByIdAsync(Guid userId, Guid id)
        {
            return await _context.OperationTypes
                .FirstOrDefaultAsync(t => 
                    t.Id == id && 
                    t.UserId == userId);
        }

        public async Task AddAsync(OperationType type)
        {
            await _context.OperationTypes.AddAsync(type);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OperationType type)
        { 
            _context.OperationTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var opType = await _context.OperationTypes.
                FirstOrDefaultAsync(t =>
                    t.Id == id && 
                    t.UserId == userId);

            if (opType != null)
            {
                _context.OperationTypes.Remove(opType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
