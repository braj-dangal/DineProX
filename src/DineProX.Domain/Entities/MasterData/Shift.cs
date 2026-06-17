using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class Shift : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        protected Shift() { }

        public Shift(Guid id, string name, TimeSpan startTime, TimeSpan endTime, string? description = null) 
            : this()
        {
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;
        }
    }
}
