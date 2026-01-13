using Azure;
using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;

namespace PersonalFinanceDataManager.Core.DTOs
{
    public class FinancialReportDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetResult => TotalIncome - TotalExpenses;
        public List<FinancialOperationDto> Operations { get; set; } = new();
    }
}
