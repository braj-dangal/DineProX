using DineProX.Dtos.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.CustomerManagement
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<CustomerDto> GetAsync(Guid id);
        Task<List<CustomerDto>> GetListAsync();
        Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input);
        Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input);
        Task DeleteAsync(Guid id);
    }
} 