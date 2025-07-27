using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.PaymentManagement
{
    public class Due : AuditedAggregateRoot<Guid>
    {
        public Guid PaymentId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingDue { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsSettled { get; set; }

        protected Due()
        {
        }

        public Due(Guid id, Guid paymentId, Guid customerId, decimal totalAmount, decimal amountPaid, decimal remainingDue, DateTime dueDate, bool isSettled = false)
            : base(id)
        {
            PaymentId = paymentId;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            AmountPaid = amountPaid;
            RemainingDue = remainingDue;
            DueDate = dueDate;
            IsSettled = isSettled;
        }
    }
} 