using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class Supplier : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PaymentTerms { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool IsActive { get; set; } = true;

        protected Supplier() { }

        public Supplier(Guid id, string name, string? contactPerson = null, string? email = null) 
            : this()
        {
            Id = id;
            Name = name;
            ContactPerson = contactPerson;
            Email = email;
        }
    }
}
