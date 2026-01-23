namespace PersonalFinanceDataManager.Core.DTOs.OperationType
{
    public class CreateOperationTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsIncome { get; set; }
        public string? Description { get; set; }
    }

}
