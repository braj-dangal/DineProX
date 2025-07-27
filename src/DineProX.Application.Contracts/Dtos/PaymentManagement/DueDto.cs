using System;

namespace DineProX.Dtos.PaymentManagement
{
    public class DueDto
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingDue { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsSettled { get; set; }
        
        // Only if needed
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
} 