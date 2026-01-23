using Microsoft.EntityFrameworkCore;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<FinancialOperation> FinancialOperations { get; set; }
        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
