using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Models
{
    public class SetActiveDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
