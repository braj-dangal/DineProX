using AutoMapper;
using DineProX.Entities.MasterData;
using DineProX.Dtos.MasterData;

namespace DineProX;

public class DineProXApplicationAutoMapperProfile : Profile
{
    public DineProXApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        // Master Data Mappings
        CreateMap<ItemCategory, ItemCategoryDto>();
        CreateMap<MenuItem, MenuItemDto>();
        CreateMap<Table, TableDto>();
        CreateMap<TableZone, TableZoneDto>();
        CreateMap<Supplier, SupplierDto>();
        CreateMap<TaxRate, TaxRateDto>();
        CreateMap<PaymentMethod, PaymentMethodDto>();
        CreateMap<Shift, ShiftDto>();
    }
}
