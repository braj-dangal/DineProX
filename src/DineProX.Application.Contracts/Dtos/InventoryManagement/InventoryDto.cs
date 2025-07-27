using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.InventoryManagement
{
    public class InventoryDto : AuditedEntityDto<Guid>
    {
        public Guid DishId { get; set; }
        public int QuantityAvailable { get; set; }
    }
} 