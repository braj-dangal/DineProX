using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class TableZone : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        protected TableZone() { }

        public TableZone(Guid id, string name, string? description = null) 
            : this()
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
