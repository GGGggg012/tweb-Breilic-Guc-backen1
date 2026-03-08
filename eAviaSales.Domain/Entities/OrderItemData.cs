using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAviaSales.Domain.Entities
{
    public class OrderItemData
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderData Order { get; set; } = null!;

        public int ProductId { get; set; }

        public string ProductInfo { get; set; } = string.Empty;

        public int Qua { get; set; }

        public decimal Price { get; set; }
    }
}
