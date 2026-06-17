using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class MenuItemDto : AuditedEntityDto<Guid>
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
        public bool IsActive { get; set; }
    }

    public class CreateMenuItemDto
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal TaxPercentage { get; set; }
        public string StockUnit { get; set; }
        public int StockQuantity { get; set; } = 0;
        public int ReorderLevel { get; set; } = 10;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Allergens { get; set; }
    }

    public class UpdateMenuItemDto
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
        public bool IsActive { get; set; }
    }
}
