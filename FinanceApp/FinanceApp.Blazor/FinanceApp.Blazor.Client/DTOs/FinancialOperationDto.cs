namespace FinanceApp.Blazor.Client.DTOs
{
    public class FinancialOperationDto
    {
        public Guid Id { get; set; }
        public Guid OperationTypeId { get; set; }

        public string OperationTypeName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsIncome { get; set; }
    }
}
