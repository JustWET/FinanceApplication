using PersonalFinanceDataManager.Core.DTOs.FinancialOperation;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services
{
    public class FinancialOperationsService : IFinancialOperationsService
    {
        private readonly IFinancialOperationsRepository _operationsRepository;
        private readonly IOperationTypesRepository _typesRepository;

        public FinancialOperationsService(IFinancialOperationsRepository operationsRepository, IOperationTypesRepository typesRepository)
        {
            _operationsRepository = operationsRepository;
            _typesRepository = typesRepository;
        }

        public async Task<List<FinancialOperation>> GetAllAsync(Guid userId)
        {
            return await _operationsRepository.GetAllAsync(userId);
        }

        public async Task<List<FinancialOperationDto>> GetAllDtosAsync(Guid userId)
        {
            var operations = await _operationsRepository.GetAllAsync(userId);

            var types = await _typesRepository.GetAllAsync(userId);

            return operations
                .Select(o =>
                {
                    var type = types.First(t => t.Id == o.TypeId);
                    return MapToDto(o, type);
                })
                .ToList();
        }


        public async Task<FinancialOperation> GetByIdAsync(Guid userId, Guid id)
        {
            var entity = await _operationsRepository.GetByIdAsync(userId, id);

            if (entity == null)
                throw new Exception("Financial operation not found.");

            return entity;
        }

        public async Task<FinancialOperationDto> GetDtoByIdAsync(Guid userId, Guid operationId)
        {
            var operation = await _operationsRepository.GetByIdAsync(userId, operationId);

            if (operation == null)
                throw new Exception("Financial operation not found.");

            var type = await _typesRepository.GetByIdAsync(userId, operation.TypeId);

            if (type == null)
                throw new InvalidOperationException("Operation type not found");

            return MapToDto(operation, type);
        }


        public async Task<FinancialOperationDto> CreateAsync(Guid userId, CreateFinancialOperationDto opDto)
        {
            if (opDto.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            var typeExists = await _typesRepository.GetByIdAsync(userId, opDto.OperationTypeId);
            if (typeExists == null)
                throw new Exception("Operation type does not exist.");

            var entity = new FinancialOperation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TypeId = opDto.OperationTypeId,
                Amount = opDto.Amount,
                Date = DateTime.Now,
                Note = opDto.Description
            };

            await _operationsRepository.AddAsync(entity);

            return MapToDto(entity, typeExists);
        }

        public async Task<FinancialOperationDto> UpdateAsync(Guid userId, UpdateFinancialOperationDto opDto)
        {
            var existing = await _operationsRepository.GetByIdAsync(userId, opDto.Id);
            if (existing == null)
                throw new Exception("Financial operation not found.");

            if (existing.UserId != userId)
                throw new Exception("You do not have permission to update this operation type.");

            if (opDto.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            var typeExists = await _typesRepository.GetByIdAsync(userId, opDto.OperationTypeId);
            if (typeExists == null)
                throw new Exception("Operation type does not exist.");

            existing.Amount = opDto.Amount;
            existing.Date = opDto.Date;
            existing.Note = opDto.Description;
            existing.TypeId = opDto.OperationTypeId;

            await _operationsRepository.UpdateAsync(existing);

            return MapToDto(existing, typeExists);
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var entity = await _operationsRepository.GetByIdAsync(userId, id);
            if (entity == null)
                throw new Exception("Financial operation not found.");

            await _operationsRepository.DeleteAsync(userId, id);
        }

        private FinancialOperationDto MapToDto(FinancialOperation entity, OperationType type)
        {
            return new FinancialOperationDto
            {
                Id = entity.Id,
                OperationTypeId = type.Id,
                Amount = entity.Amount,
                Date = entity.Date,
                Description = entity.Note,
                IsIncome = type.IsIncome,
                OperationTypeName = type.Name
            };
        }
    }
}
