using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using eAviaSales.Domain.Entities;

namespace eAviaSales.Domain.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
