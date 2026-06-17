using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class ShiftDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string StartTime { get; set; } // HH:mm format
        public string EndTime { get; set; } // HH:mm format
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateShiftDto
    {
        public string Name { get; set; }
        public string StartTime { get; set; } // HH:mm format
        public string EndTime { get; set; } // HH:mm format
        public string? Description { get; set; }
    }

    public class UpdateShiftDto
    {
        public string Name { get; set; }
        public string StartTime { get; set; } // HH:mm format
        public string EndTime { get; set; } // HH:mm format
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
