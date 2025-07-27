using System;

namespace DineProX.Dtos.ReportManagement
{
    public class InventoryStatusDto
    {
        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public int QuantityAvailable { get; set; }
    }
} 