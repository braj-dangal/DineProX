using System;

namespace DineProX.Dtos.ReportManagement
{
    public class OutstandingDueSummaryDto
    {
        public int TotalUnpaidCustomers { get; set; }
        public decimal TotalOutstandingAmount { get; set; }
    }
} 