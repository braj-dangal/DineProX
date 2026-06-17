using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class ItemCategoryDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateItemCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateItemCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
