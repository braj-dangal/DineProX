using System;

namespace DineProX.Dtos.PaymentManagement
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalBill { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime Date { get; set; }
        
        // Only if needed
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
    }
} 