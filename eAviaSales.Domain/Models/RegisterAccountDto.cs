using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Models
{
    /// <summary>
    /// Публичная регистрация. Роль всегда User, кроме одноразового bootstrap первого админа.
    /// </summary>
    public class RegisterAccountDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Секрет для создания первого админа, пока в БД нет пользователей с Role=Admin.
        /// </summary>
        public string? BootstrapSecret { get; set; }
    }
}
