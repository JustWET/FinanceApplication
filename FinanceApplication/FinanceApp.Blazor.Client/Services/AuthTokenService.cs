using Blazored.LocalStorage;

namespace FinanceApp.Blazor.Client.Services
{
    public class AuthTokenService
    {
        private const string TokenKey = "authToken";
        private readonly ILocalStorageService _localStorage;

        public AuthTokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }

        public async Task<bool> HasTokenAsync()
        {
            return await _localStorage.ContainKeyAsync(TokenKey);
        }
    }
}
