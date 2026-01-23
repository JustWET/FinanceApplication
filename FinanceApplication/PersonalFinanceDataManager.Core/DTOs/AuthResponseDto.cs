namespace PersonalFinanceDataManager.Core.DTOs
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
    }

}
