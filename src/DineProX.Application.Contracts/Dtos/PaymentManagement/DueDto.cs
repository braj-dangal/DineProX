using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.PaymentManagement
{
    public class DueDto : AuditedEntityDto<Guid>
    {
        public Guid PaymentId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal RemainingDue { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsSettled { get; set; }
    }
} 