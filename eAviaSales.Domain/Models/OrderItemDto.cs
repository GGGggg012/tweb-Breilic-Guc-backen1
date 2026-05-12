using System.ComponentModel.DataAnnotations;

namespace eAviaSales.Domain.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public string ProductInfo { get; set; } = string.Empty;

        [Required]
        [Range(1, 100)]
        public int Qua { get; set; }

        public decimal Price { get; set; }
    }
}
