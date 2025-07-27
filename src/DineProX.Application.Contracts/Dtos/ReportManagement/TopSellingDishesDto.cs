using System;

namespace DineProX.Dtos.ReportManagement
{
    public class TopSellingDishesDto
    {
        public Guid DishId { get; set; }
        public string DishName { get; set; }
        public int TotalQuantitySold { get; set; }
    }
} 