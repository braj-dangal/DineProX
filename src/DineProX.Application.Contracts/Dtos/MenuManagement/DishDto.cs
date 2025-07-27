using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MenuManagement
{
    public class DishDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
} 