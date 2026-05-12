using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAviaSales.Domain.Entities
{
    public class OrderData
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [InverseProperty("Order")]
        public List<OrderItemData> Items { get; set; } = new List<OrderItemData>();

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; }
    }
}
