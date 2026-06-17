using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class TaxRate : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        protected TaxRate() { }

        public TaxRate(Guid id, string name, decimal rate, string? description = null) 
            : this()
        {
            Id = id;
            Name = name;
            Rate = rate;
            Description = description;
        }
    }
}
