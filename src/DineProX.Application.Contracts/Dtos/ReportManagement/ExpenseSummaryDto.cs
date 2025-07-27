using System;
using System.Collections.Generic;

namespace DineProX.Dtos.ReportManagement
{
    public class ExpenseSummaryDto
    {
        public decimal TotalExpenses { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Dictionary<string, decimal> BreakdownByCategory { get; set; }
    }
} 