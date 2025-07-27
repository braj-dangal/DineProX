using DineProX.Dtos.ExpenseManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.ExpenseManagement
{
    public interface IExpenseAppService : IApplicationService
    {
        Task<ExpenseDto> CreateAsync(CreateUpdateExpenseDto input);
        Task<ExpenseDto> UpdateAsync(Guid id, CreateUpdateExpenseDto input);
        Task DeleteAsync(Guid id);
        Task<ExpenseDto> GetAsync(Guid id);
        Task<List<ExpenseDto>> GetListAsync();
    }
} 