namespace FinanceApp.Blazor.Client.DTOs
{
    public class FinancialOperationFormDto
    {
        public Guid Id { get; set; }
        public Guid OperationTypeId { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }

}
