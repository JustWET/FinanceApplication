using PersonalFinanceDataManager.Core.DTOs;
using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IFinancialOperationsRepository _operationsRepository;
        private readonly IOperationTypesRepository _typesRepository;

        public ReportsService(IFinancialOperationsRepository operationsRepo, IOperationTypesRepository typesRepo)
        {
            _operationsRepository = operationsRepo;
            _typesRepository = typesRepo;
        }

        public async Task<FinancialReportDto> GetDailyReportAsync(Guid userId, DateTime date)
        {
            var operations = await _operationsRepository.GetAllAsync(userId);
            var types = await _typesRepository.GetAllAsync(userId);

            var filtered = operations
                .Where(o => !o.IsDeleted && o.Date.Date == date.Date);

            var dtos = MapToDtoList(filtered, types);

            var totalIncome = dtos.Where(o => o.IsIncome).Sum(o => o.Amount);
            var totalExpenses = dtos.Where(o => !o.IsIncome).Sum(o => o.Amount);

            return new FinancialReportDto
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Operations = dtos
            };
        }

        public async Task<FinancialReportDto> GetPeriodReportAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            var operations = await _operationsRepository.GetAllAsync(userId);
            var types = await _typesRepository.GetAllAsync(userId);

            var filtered = operations
                .Where(o =>
                    !o.IsDeleted &&
                    o.Date.Date >= startDate.Date &&
                    o.Date.Date <= endDate.Date);

            var dtos = MapToDtoList(filtered, types);

            var totalIncome = dtos.Where(o => o.IsIncome).Sum(o => o.Amount);
            var totalExpenses = dtos.Where(o => !o.IsIncome).Sum(o => o.Amount);

            return new FinancialReportDto
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Operations = dtos
            };
        }

        private IEnumerable<FinancialOperationDto> MapToDtoList(
            IEnumerable<FinancialOperation> operations,
            List<OperationType> types)
        {
            return operations
                .Select(op =>
                {
                    var type = types.First(t => t.Id == op.TypeId);
                    return new FinancialOperationDto
                    {
                        Id = op.Id,
                        OperationTypeId = type.Id,
                        OperationTypeName = type.Name,
                        Amount = op.Amount,
                        Date = op.Date,
                        Description = op.Note,
                        IsIncome = type.IsIncome
                    };
                }).AsEnumerable();
        }
    }
}
