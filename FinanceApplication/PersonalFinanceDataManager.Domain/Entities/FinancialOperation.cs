
namespace PersonalFinanceDataManager.Domain.Entities
{
    public class FinancialOperation
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
