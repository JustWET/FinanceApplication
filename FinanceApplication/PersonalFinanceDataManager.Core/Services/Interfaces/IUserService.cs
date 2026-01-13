using PersonalFinanceDataManager.Domain.Entities;

namespace PersonalFinanceDataManager.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<string> RegisterAsync(string username, string password);
        Task<string?> LoginAsync(string username, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }

}
