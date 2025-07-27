using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.InventoryManagement
{
    public class Purchase : AuditedAggregateRoot<Guid>
    {
        public Guid DishId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; }

        protected Purchase()
        {
        }

        public Purchase(Guid id, Guid dishId, int quantity, decimal purchasePrice, string supplierName, DateTime purchaseDate)
            : base(id)
        {
            DishId = dishId;
            Quantity = quantity;
            PurchasePrice = purchasePrice;
            SupplierName = supplierName;
            PurchaseDate = purchaseDate;
        }
    }
} 