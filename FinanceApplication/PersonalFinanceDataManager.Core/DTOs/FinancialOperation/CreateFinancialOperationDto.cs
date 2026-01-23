namespace PersonalFinanceDataManager.Core.DTOs.FinancialOperation
{
    public class CreateFinancialOperationDto
    {
        public required Guid OperationTypeId { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
