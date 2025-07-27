using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.OrderManagement
{
    public class CreateOrderDto : EntityDto<Guid?>
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
        public decimal Discount { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item")]
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
} 