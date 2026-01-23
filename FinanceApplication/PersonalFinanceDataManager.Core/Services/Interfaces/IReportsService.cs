using PersonalFinanceDataManager.Core.DTOs;

namespace PersonalFinanceDataManager.Core.Services.Interfaces
{
    public interface IReportsService
    {
        Task<FinancialReportDto> GetDailyReportAsync(Guid userId, DateTime date);
        Task<FinancialReportDto> GetPeriodReportAsync(Guid userId, DateTime startDate, DateTime endDate);
    }
}
