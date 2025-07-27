using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.CustomerManagement
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid? UserId { get; set; }

        protected Customer()
        {
        }

        public Customer(Guid id, string name, string phoneNumber, string address, Guid? userId = null)
            : base(id)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Address = address;
            UserId = userId;
        }
    }
} 