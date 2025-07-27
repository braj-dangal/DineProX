using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using DineProX.Enums;

namespace DineProX.Entities.OrderManagement
{
    public class Order : AuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount => TotalAmount - Discount;
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        protected Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(Guid id, Guid customerId, decimal totalAmount, decimal discount, DateTime orderDate, OrderStatus status = OrderStatus.Pending)
            : base(id)
        {
            CustomerId = customerId;
            TotalAmount = totalAmount;
            Discount = discount;
            OrderDate = orderDate;
            Status = status;
            OrderItems = new List<OrderItem>();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            OrderItems.Add(orderItem);
            RecalculateTotalAmount();
        }

        public void RemoveOrderItem(Guid orderItemId)
        {
            var orderItem = OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
            if (orderItem != null)
            {
                OrderItems.Remove(orderItem);
                RecalculateTotalAmount();
            }
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = OrderItems.Sum(oi => oi.Total);
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }

        public void MarkAsPaid()
        {
            Status = OrderStatus.Paid;
        }
    }
} 