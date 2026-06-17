using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class Table : FullAuditedAggregateRoot<Guid>
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public Guid? ZoneId { get; set; }
        public TableStatus Status { get; set; } = TableStatus.Free;
        public bool IsActive { get; set; } = true;

        protected Table() { }

        public Table(Guid id, string tableNumber, int capacity, Guid? zoneId = null) 
            : this()
        {
            Id = id;
            TableNumber = tableNumber;
            Capacity = capacity;
            ZoneId = zoneId;
        }

        public void MarkAsOccupied() => Status = TableStatus.Occupied;
        public void MarkAsFree() => Status = TableStatus.Free;
        public void MarkAsReserved() => Status = TableStatus.Reserved;
    }

    public enum TableStatus
    {
        Free = 0,
        Occupied = 1,
        Reserved = 2
    }
}
