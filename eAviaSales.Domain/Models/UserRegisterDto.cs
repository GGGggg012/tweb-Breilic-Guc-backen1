using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Models
{
    /// <summary>
    /// Обновление профиля (без смены роли).
    /// </summary>
    public class UserRegisterDto
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
    }
}
