using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.PaymentManagement
{
    public class Payment : AuditedAggregateRoot<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalBill { get; set; }
        public DateTime Date { get; set; }

        public decimal DueAmount => TotalBill - AmountPaid - Discount;

        protected Payment()
        {
        }

        public Payment(Guid id, Guid orderId, Guid customerId, decimal amountPaid, decimal discount, decimal totalBill, DateTime date)
            : base(id)
        {
            OrderId = orderId;
            CustomerId = customerId;
            AmountPaid = amountPaid;
            Discount = discount;
            TotalBill = totalBill;
            Date = date;
        }
    }
} 