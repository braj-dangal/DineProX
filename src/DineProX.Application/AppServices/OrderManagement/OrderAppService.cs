using DineProX.Dtos.OrderManagement;
using DineProX.Entities.OrderManagement;
using DineProX.Interfaces.OrderManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DineProX.Enums;
using DineProX.Entities.MenuManagement;

namespace DineProX.AppServices.OrderManagement
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<Dish, Guid> _dishRepository;

        public OrderAppService(
            IRepository<Order, Guid> orderRepository,
            IRepository<OrderItem, Guid> orderItemRepository,
            IRepository<Dish, Guid> dishRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _dishRepository = dishRepository;
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Order requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Order requested for ID: {id}");

            var order = await _orderRepository.GetAsync(id);
            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Order List requested by User: {CurrentUser.Id}");

            var orders = await _orderRepository.GetListAsync();
            return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            Logger.LogInformation($"Create Order requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Create Order requested for: {input}");

            // Validate business rules
            if (input.OrderItems == null || !input.OrderItems.Any())
            {
                throw new UserFriendlyException("Order must contain at least one item.");
            }

            if (input.Discount < 0)
            {
                throw new UserFriendlyException("Discount cannot be negative.");
            }

            // Calculate total amount and create order items
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in input.OrderItems)
            {
                // Get dish price
                var dish = await _dishRepository.GetAsync(itemDto.DishId);
                
                // Create order item
                var orderItem = new OrderItem(
                    GuidGenerator.Create(),
                    Guid.Empty, // Will be set after order creation
                    itemDto.DishId,
                    itemDto.Quantity,
                    dish.Price
                );

                totalAmount += orderItem.Total;
                orderItems.Add(orderItem);
            }

            // Validate discount doesn't exceed total amount
            if (input.Discount > totalAmount)
            {
                throw new UserFriendlyException("Discount cannot exceed total amount.");
            }

            // Create the order
            var order = new Order(
                GuidGenerator.Create(),
                input.CustomerId,
                totalAmount,
                input.Discount,
                Clock.Now,
                OrderStatus.Pending
            );

            // Insert the order
            var createdOrder = await _orderRepository.InsertAsync(order);

            // Create and insert order items
            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = createdOrder.Id;
                await _orderItemRepository.InsertAsync(orderItem);
            }

            Logger.LogInformation($"Order created successfully. Order ID: {createdOrder.Id}, Customer ID: {input.CustomerId}, Total Amount: {totalAmount}, Final Amount: {createdOrder.FinalAmount}");

            return ObjectMapper.Map<Order, OrderDto>(createdOrder);
        }

        public async Task CancelAsync(Guid id)
        {
            Logger.LogInformation($"Cancel Order requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Cancel Order requested for ID: {id}");

            var order = await _orderRepository.GetAsync(id);

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new UserFriendlyException("Order is already cancelled.");
            }

            if (order.Status == OrderStatus.Paid)
            {
                throw new UserFriendlyException("Cannot cancel a paid order.");
            }

            order.Cancel();
            await _orderRepository.UpdateAsync(order);

            Logger.LogInformation($"Order {id} cancelled successfully.");
        }

        public async Task MarkAsPaidAsync(Guid id)
        {
            Logger.LogInformation($"Mark Order as Paid requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Mark Order as Paid requested for ID: {id}");

            var order = await _orderRepository.GetAsync(id);

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new UserFriendlyException("Cannot mark a cancelled order as paid.");
            }

            if (order.Status == OrderStatus.Paid)
            {
                throw new UserFriendlyException("Order is already marked as paid.");
            }

            order.MarkAsPaid();
            await _orderRepository.UpdateAsync(order);

            Logger.LogInformation($"Order {id} marked as paid successfully.");
        }
    }
} 