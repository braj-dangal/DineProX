using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class ItemCategory : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        protected ItemCategory() { }

        public ItemCategory(Guid id, string name, string? description = null, int displayOrder = 0) 
            : this()
        {
            Id = id;
            Name = name;
            Description = description;
            DisplayOrder = displayOrder;
        }
    }
}
