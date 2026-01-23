namespace FinanceApp.Blazor.Client.DTOs
{
    public class OperationTypeLookupDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsIncome { get; set; }
    }
}
