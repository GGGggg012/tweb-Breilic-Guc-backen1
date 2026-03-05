using System;
using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Entities
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        public DateTime RegisteredOn { get; set; }

        public void SetPasswordHash(string password)
        {
            var bytes = System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password));
            PasswordHash = Convert.ToHexString(bytes).ToLower();
        }

        public bool CheckPassword(string password)
        {
            var bytes = System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password));
            return PasswordHash == Convert.ToHexString(bytes).ToLower();
        }
    }
}
