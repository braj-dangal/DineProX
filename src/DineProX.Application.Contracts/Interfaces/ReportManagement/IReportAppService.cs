using DineProX.Dtos.ReportManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.ReportManagement
{
    public interface IReportAppService : IApplicationService
    {
        Task<DailySalesReportDto> GetDailySalesAsync(DateTime date);
        Task<OutstandingDueSummaryDto> GetOutstandingDuesSummaryAsync();
        Task<List<InventoryStatusDto>> GetInventoryStatusAsync();
        Task<List<TopSellingDishesDto>> GetTopSellingDishesAsync(DateTime from, DateTime to, int top = 5);
        Task<ExpenseSummaryDto> GetExpenseSummaryAsync(DateTime from, DateTime to);
    }
} 