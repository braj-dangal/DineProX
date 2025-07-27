using AutoMapper;
using DineProX.Dtos.CustomerManagement;
using DineProX.Entities.CustomerManagement;

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
    }
}
