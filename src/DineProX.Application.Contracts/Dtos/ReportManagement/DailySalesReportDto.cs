using System;

namespace DineProX.Dtos.ReportManagement
{
    public class DailySalesReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalDuesCreated { get; set; }
        public decimal TotalAmountPaid { get; set; }
    }
} 