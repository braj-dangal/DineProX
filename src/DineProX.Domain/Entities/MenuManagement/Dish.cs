using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MenuManagement
{
    public class Dish : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        protected Dish()
        {
        }

        public Dish(Guid id, string name, string description, decimal price, bool isAvailable = true)
            : base(id)
        {
            Name = name;
            Description = description;
            Price = price;
            IsAvailable = isAvailable;
        }
    }
} 