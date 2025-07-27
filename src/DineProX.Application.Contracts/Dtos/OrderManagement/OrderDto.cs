using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using DineProX.Enums;

namespace DineProX.Dtos.OrderManagement
{
    public class OrderDto : AuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
} 