using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.InventoryManagement
{
    public class PurchaseDto : AuditedEntityDto<Guid>
    {
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
} 