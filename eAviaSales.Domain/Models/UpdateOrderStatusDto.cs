using System.ComponentModel.DataAnnotations;
using eAviaSales.Domain.Entities;

namespace eAviaSales.Domain.Models
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
