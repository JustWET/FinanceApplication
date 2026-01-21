using PersonalFinanceDataManager.Core.DTOs.OperationType;
using PersonalFinanceDataManager.Core.Services.Interfaces;
using PersonalFinanceDataManager.Data.Repositories.Interfaces;
using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services
{
    public class OperationTypesService : IOperationTypesService
    {
        private readonly IFinancialOperationsRepository _operationsRepository;
        private readonly IOperationTypesRepository _typesRepository;

        public OperationTypesService(IFinancialOperationsRepository operationsRepository, IOperationTypesRepository typesRepository)
        {
            _operationsRepository = operationsRepository;
            _typesRepository = typesRepository;
        }

        public async Task<List<OperationTypeDto>> GetAllAsync(Guid userId)
        {
            var types = await _typesRepository.GetAllAsync(userId);
            return types.Select(MapToDto).ToList();
        }

        public async Task<OperationTypeDto> GetByIdAsync(Guid userId, Guid id)
        {
            var entity = await _typesRepository.GetByIdAsync(userId, id);

            if (entity == null)
                throw new Exception("Operation type not found.");

            return MapToDto(entity);
        }

        public async Task<OperationTypeDto> CreateAsync(Guid userId, CreateOperationTypeDto typeDto)
        {
            if (string.IsNullOrWhiteSpace(typeDto.Name))
                throw new Exception("Name cannot be empty.");

            if (await _typesRepository.ExistsWithNameAsync(userId, typeDto.Name))
                throw new Exception("Operation type with this name already exists");

            var entity = new OperationType
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = typeDto.Name,
                IsIncome = typeDto.IsIncome,
                Description = typeDto.Description
            };

            await _typesRepository.AddAsync(entity);
            return MapToDto(entity);
        }

        public async Task<OperationTypeDto> UpdateAsync(Guid userId, UpdateOperationTypeDto typeDto)
        {
            var existing = await _typesRepository.GetByIdAsync(userId, typeDto.Id);

            if (existing == null)
                throw new Exception("Operation type not found.");

            if (await _typesRepository.ExistsWithNameAsync(userId, typeDto.Name, typeDto.Id))
                throw new Exception("Operation type with this name already exists");

            if (existing.UserId != userId)
                throw new Exception("You do not have permission to update this operation type.");

            existing.Name = typeDto.Name;
            existing.Description = typeDto.Description;
            existing.IsIncome = typeDto.IsIncome;

            await _typesRepository.UpdateAsync(existing);

            return MapToDto(existing);
        }

        public async Task DeleteAsync(Guid userId, Guid id)
        {
            var existing = await _typesRepository.GetByIdAsync(userId, id);
            if (existing == null)
                throw new Exception("Operation type not found.");

            if (existing.UserId != userId)
                throw new Exception("You do not have permission to update this operation type.");

            var hasLinkedOperations = await _operationsRepository.ExistsByTypeIdAsync(userId, id);
            if (hasLinkedOperations)
                throw new Exception("Cannot delete operation type because it is used by existing financial operations.");

            await _typesRepository.DeleteAsync(userId, id);
        }

        public async Task<List<OperationTypeUsageDto>> GetOperationTypeUsageAsync(Guid userId)
        {
            var operations = await _operationsRepository.GetAllAsync(userId);
                
            return operations
                .Where(o => o.UserId == userId)
                .GroupBy(o => o.TypeId)
                .Select(g => new OperationTypeUsageDto
                {
                    OperationTypeId = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }

        private OperationTypeDto MapToDto(OperationType entity)
        {
            return new OperationTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsIncome = entity.IsIncome,
                Description = entity.Description
            };
        }
    }
}
