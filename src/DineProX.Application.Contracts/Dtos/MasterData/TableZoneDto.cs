using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class TableZoneDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTableZoneDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateTableZoneDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
