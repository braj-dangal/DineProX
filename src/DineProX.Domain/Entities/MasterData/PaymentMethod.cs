using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class PaymentMethod : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public PaymentType Type { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        protected PaymentMethod() { }

        public PaymentMethod(Guid id, string name, PaymentType type, string? description = null) 
            : this()
        {
            Id = id;
            Name = name;
            Type = type;
            Description = description;
        }
    }

    public enum PaymentType
    {
        Cash = 0,
        Card = 1,
        Wallet = 2,
        Cheque = 3,
        BankTransfer = 4
    }
}
