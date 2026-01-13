namespace FinanceApp.Blazor.Client.DTOs
{
    public class OperationTypeDto
    {
        public Guid Id { get; set; }

        public bool IsIncome { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
