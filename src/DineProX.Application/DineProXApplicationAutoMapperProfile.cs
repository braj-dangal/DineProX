using AutoMapper;
using DineProX.Dtos.CustomerManagement;
using DineProX.Dtos.InventoryManagement;
using DineProX.Dtos.PaymentManagement;
using DineProX.Entities.CustomerManagement;
using DineProX.Entities.InventoryManagement;
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
    }
}
