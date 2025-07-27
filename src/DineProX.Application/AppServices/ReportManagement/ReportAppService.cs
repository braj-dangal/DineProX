using DineProX.Dtos.ReportManagement;
using DineProX.Entities.ExpenseManagement;
using DineProX.Entities.InventoryManagement;
using DineProX.Entities.MenuManagement;
using DineProX.Entities.OrderManagement;
using DineProX.Entities.PaymentManagement;
using DineProX.Interfaces.ReportManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DineProX.Enums;

namespace DineProX.AppServices.ReportManagement
{
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Payment, Guid> _paymentRepository;
        private readonly IRepository<Due, Guid> _dueRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<Dish, Guid> _dishRepository;
        private readonly IRepository<Expense, Guid> _expenseRepository;

        public ReportAppService(
            IRepository<Order, Guid> orderRepository,
            IRepository<Payment, Guid> paymentRepository,
            IRepository<Due, Guid> dueRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IRepository<OrderItem, Guid> orderItemRepository,
            IRepository<Dish, Guid> dishRepository,
            IRepository<Expense, Guid> expenseRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _dueRepository = dueRepository;
            _inventoryRepository = inventoryRepository;
            _orderItemRepository = orderItemRepository;
            _dishRepository = dishRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<DailySalesReportDto> GetDailySalesAsync(DateTime date)
        {
            Logger.LogInformation($"Get Daily Sales Report requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Daily Sales Report requested for date: {date:yyyy-MM-dd}");

            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            // Get orders for the specified date
            var orders = await _orderRepository.GetListAsync(o => 
                o.OrderDate >= startOfDay && o.OrderDate < endOfDay);

            // Get payments for the specified date
            var payments = await _paymentRepository.GetListAsync(p => 
                p.Date >= startOfDay && p.Date < endOfDay);

            // Get dues created for the specified date
            var duesCreated = await _dueRepository.GetListAsync(d => 
                d.CreationTime >= startOfDay && d.CreationTime < endOfDay);

            var report = new DailySalesReportDto
            {
                Date = date,
                TotalSales = orders.Where(o => o.Status == OrderStatus.Paid).Sum(o => o.FinalAmount),
                TotalOrders = orders.Count,
                TotalDuesCreated = duesCreated.Sum(d => d.RemainingDue),
                TotalAmountPaid = payments.Sum(p => p.AmountPaid)
            };

            Logger.LogInformation($"Daily Sales Report generated for {date:yyyy-MM-dd}. Total Sales: {report.TotalSales}, Total Orders: {report.TotalOrders}");

            return report;
        }

        public async Task<OutstandingDueSummaryDto> GetOutstandingDuesSummaryAsync()
        {
            Logger.LogInformation($"Get Outstanding Dues Summary requested by User: {CurrentUser.Id}");

            // Get all unsettled dues
            var unsettledDues = await _dueRepository.GetListAsync(d => !d.IsSettled);

            var summary = new OutstandingDueSummaryDto
            {
                TotalUnpaidCustomers = unsettledDues.Select(d => d.CustomerId).Distinct().Count(),
                TotalOutstandingAmount = unsettledDues.Sum(d => d.RemainingDue)
            };

            Logger.LogInformation($"Outstanding Dues Summary generated. Unpaid Customers: {summary.TotalUnpaidCustomers}, Outstanding Amount: {summary.TotalOutstandingAmount}");

            return summary;
        }

        public async Task<List<InventoryStatusDto>> GetInventoryStatusAsync()
        {
            Logger.LogInformation($"Get Inventory Status requested by User: {CurrentUser.Id}");

            // Get all inventory items with dish information
            var inventories = await _inventoryRepository.GetListAsync();
            var dishes = await _dishRepository.GetListAsync();

            var inventoryStatus = inventories.Select(inv =>
            {
                var dish = dishes.FirstOrDefault(d => d.Id == inv.DishId);
                return new InventoryStatusDto
                {
                    DishId = inv.DishId,
                    DishName = dish?.Name ?? "Unknown Dish",
                    QuantityAvailable = inv.QuantityAvailable
                };
            }).ToList();

            Logger.LogInformation($"Inventory Status generated for {inventoryStatus.Count} items");

            return inventoryStatus;
        }

        public async Task<List<TopSellingDishesDto>> GetTopSellingDishesAsync(DateTime from, DateTime to, int top = 5)
        {
            Logger.LogInformation($"Get Top Selling Dishes requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Top Selling Dishes requested from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}, top: {top}");

            // Get orders within the date range
            var orders = await _orderRepository.GetListAsync(o => 
                o.OrderDate >= from && o.OrderDate <= to && o.Status == OrderStatus.Paid);

            var orderIds = orders.Select(o => o.Id).ToList();

            // Get order items for these orders
            var orderItems = await _orderItemRepository.GetListAsync(oi => orderIds.Contains(oi.OrderId));

            // Get dish information
            var dishes = await _dishRepository.GetListAsync();

            // Group by dish and calculate total quantity sold
            var topSellingDishes = orderItems
                .GroupBy(oi => oi.DishId)
                .Select(g =>
                {
                    var dish = dishes.FirstOrDefault(d => d.Id == g.Key);
                    return new TopSellingDishesDto
                    {
                        DishId = g.Key,
                        DishName = dish?.Name ?? "Unknown Dish",
                        TotalQuantitySold = g.Sum(oi => oi.Quantity)
                    };
                })
                .OrderByDescending(d => d.TotalQuantitySold)
                .Take(top)
                .ToList();

            Logger.LogInformation($"Top Selling Dishes generated. Found {topSellingDishes.Count} dishes");

            return topSellingDishes;
        }

        public async Task<ExpenseSummaryDto> GetExpenseSummaryAsync(DateTime from, DateTime to)
        {
            Logger.LogInformation($"Get Expense Summary requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Expense Summary requested from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}");

            // Get expenses within the date range
            var expenses = await _expenseRepository.GetListAsync(e => 
                e.ExpenseDate >= from && e.ExpenseDate <= to);

            var breakdownByCategory = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(e => e.Amount)
                );

            var summary = new ExpenseSummaryDto
            {
                TotalExpenses = expenses.Sum(e => e.Amount),
                FromDate = from,
                ToDate = to,
                BreakdownByCategory = breakdownByCategory
            };

            Logger.LogInformation($"Expense Summary generated. Total Expenses: {summary.TotalExpenses}, Categories: {breakdownByCategory.Count}");

            return summary;
        }
    }
} 