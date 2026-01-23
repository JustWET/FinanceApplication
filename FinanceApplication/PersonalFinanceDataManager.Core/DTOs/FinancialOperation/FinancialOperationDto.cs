namespace PersonalFinanceDataManager.Core.DTOs.FinancialOperation
{
    public class FinancialOperationDto
    {
        public required Guid Id { get; set; }
        public required Guid OperationTypeId { get; set; }

        public required string OperationTypeName { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }
        public required string Description { get; set; }
        public required bool IsIncome { get; set; }
    }
}
