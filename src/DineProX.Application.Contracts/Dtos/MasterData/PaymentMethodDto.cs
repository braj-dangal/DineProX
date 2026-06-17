using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class PaymentMethodDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public int Type { get; set; } // 0: Cash, 1: Card, 2: Wallet, 3: Cheque, 4: BankTransfer
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreatePaymentMethodDto
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string? Description { get; set; }
    }

    public class UpdatePaymentMethodDto
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
