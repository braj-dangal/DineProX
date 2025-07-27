using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.ExpenseManagement
{
    public class ExpenseDto : AuditedEntityDto<Guid>
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }
    }
} 