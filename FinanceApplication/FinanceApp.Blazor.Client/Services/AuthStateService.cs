namespace FinanceApp.Blazor.Client.Services
{
    public class AuthStateService
    {
        public bool IsAuthenticated { get; private set; }

        public event Action? OnChange;

        public void SetAuthenticated(bool value)
        {
            IsAuthenticated = value;
            OnChange?.Invoke();
        }
    }

}
