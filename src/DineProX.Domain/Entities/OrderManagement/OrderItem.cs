using System;
using Volo.Abp.Domain.Entities;

namespace DineProX.Entities.OrderManagement
{
    public class OrderItem : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;

        protected OrderItem()
        {
        }

        public OrderItem(Guid id, Guid orderId, Guid dishId, int quantity, decimal unitPrice)
            : base(id)
        {
            OrderId = orderId;
            DishId = dishId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
} 