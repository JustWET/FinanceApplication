using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }

}
