using System;
using System.ComponentModel.DataAnnotations;

namespace DineProX.Dtos.OrderManagement
{
    public class CreateOrderItemDto
    {
        [Required]
        public Guid DishId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
} 