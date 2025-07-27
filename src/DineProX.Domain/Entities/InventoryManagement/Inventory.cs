using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.InventoryManagement
{
    public class Inventory : AuditedAggregateRoot<Guid>
    {
        public Guid DishId { get; set; }
        public int QuantityAvailable { get; set; }

        protected Inventory()
        {
        }

        public Inventory(Guid id, Guid dishId, int quantityAvailable)
            : base(id)
        {
            DishId = dishId;
            QuantityAvailable = quantityAvailable;
        }
    }
} 