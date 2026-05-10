using System.ComponentModel.DataAnnotations;
using eAviaSales.Domain.Entities;

namespace eAviaSales.Domain.Models
{
    public class SetUserRoleDto
    {
        [Required]
        public UserRole Role { get; set; }
    }
}
