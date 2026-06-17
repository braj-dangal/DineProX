using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.MasterData
{
    public class MenuItem : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal TaxPercentage { get; set; }
        public string StockUnit { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Allergens { get; set; }
        public bool IsActive { get; set; } = true;

        protected MenuItem() { }

        public MenuItem(
            Guid id, 
            string name, 
            Guid categoryId, 
            decimal price, 
            decimal taxPercentage,
            string stockUnit,
            int reorderLevel = 10) 
            : this()
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Price = price;
            TaxPercentage = taxPercentage;
            StockUnit = stockUnit;
            ReorderLevel = reorderLevel;
        }
    }
}
