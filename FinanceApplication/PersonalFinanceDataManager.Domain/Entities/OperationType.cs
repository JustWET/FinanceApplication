
namespace PersonalFinanceDataManager.Domain.Entities
{
    public class OperationType
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public bool IsIncome { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
