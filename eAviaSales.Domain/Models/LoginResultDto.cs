namespace eAviaSales.Domain.Models
{
    /// <summary>
    /// Результат успешного логина — токен и данные пользователя для фронта.
    /// </summary>
    public class LoginResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
