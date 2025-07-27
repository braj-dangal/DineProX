using AutoMapper;
using DineProX.Dtos.CustomerManagement;
using DineProX.Dtos.ExpenseManagement;
using DineProX.Dtos.InventoryManagement;
using DineProX.Dtos.MenuManagement;
using DineProX.Dtos.OrderManagement;
using DineProX.Dtos.PaymentManagement;
using DineProX.Dtos.ReportManagement;
using DineProX.Entities.CustomerManagement;
using DineProX.Entities.ExpenseManagement;
using DineProX.Entities.InventoryManagement;
using DineProX.Entities.MenuManagement;
using DineProX.Entities.OrderManagement;
using DineProX.Entities.PaymentManagement;

namespace DineProX;

public class DineProXApplicationAutoMapperProfile : Profile
{
    public DineProXApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();

        // Payment mappings
        CreateMap<Payment, PaymentDto>();
        CreateMap<CreateUpdatePaymentDto, Payment>();
        CreateMap<Due, DueDto>();
        CreateMap<SettleDueDto, Due>();

        // Inventory mappings
        CreateMap<Purchase, PurchaseDto>();
        CreateMap<CreatePurchaseDto, Purchase>();
        CreateMap<Inventory, InventoryDto>();
        CreateMap<UpdateInventoryDto, Inventory>();

        // Order mappings
        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<Dish, DishDto>();

        // Expense mappings
        CreateMap<Expense, ExpenseDto>();
        CreateMap<CreateUpdateExpenseDto, Expense>();

        // Report mappings (read-only DTOs, no entity mapping needed)
        // These DTOs are populated manually in the ReportAppService
    }
}
