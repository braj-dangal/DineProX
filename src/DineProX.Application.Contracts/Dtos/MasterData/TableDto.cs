using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class TableDto : AuditedEntityDto<Guid>
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public Guid? ZoneId { get; set; }
        public int Status { get; set; } // 0: Free, 1: Occupied, 2: Reserved
        public bool IsActive { get; set; }
    }

    public class CreateTableDto
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public Guid? ZoneId { get; set; }
    }

    public class UpdateTableDto
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public Guid? ZoneId { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
    }
}
