using DineProX.Dtos.ExpenseManagement;
using DineProX.Entities.ExpenseManagement;
using DineProX.Interfaces.ExpenseManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace DineProX.AppServices.ExpenseManagement
{
    public class ExpenseAppService : ApplicationService, IExpenseAppService
    {
        private readonly IRepository<Expense, Guid> _expenseRepository;

        public ExpenseAppService(IRepository<Expense, Guid> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<ExpenseDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Expense requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Expense requested for ID: {id}");

            var expense = await _expenseRepository.GetAsync(id);
            return ObjectMapper.Map<Expense, ExpenseDto>(expense);
        }

        public async Task<List<ExpenseDto>> GetListAsync()
        {
            Logger.LogInformation($"Get Expense List requested by User: {CurrentUser.Id}");

            var expenses = await _expenseRepository.GetListAsync();
            return ObjectMapper.Map<List<Expense>, List<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto> CreateAsync(CreateUpdateExpenseDto input)
        {
            Logger.LogInformation($"Create Expense requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Create Expense requested for: {input}");

            // Validate business rules
            if (string.IsNullOrWhiteSpace(input.Description))
            {
                throw new UserFriendlyException("Description is required.");
            }

            if (input.Amount <= 0)
            {
                throw new UserFriendlyException("Amount must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(input.Category))
            {
                throw new UserFriendlyException("Category is required.");
            }

            // Create the expense
            var expense = new Expense(
                GuidGenerator.Create(),
                input.Description.Trim(),
                input.Amount,
                input.ExpenseDate,
                input.Category.Trim()
            );

            var createdExpense = await _expenseRepository.InsertAsync(expense);

            Logger.LogInformation($"Expense created successfully. Expense ID: {createdExpense.Id}, Amount: {input.Amount}, Category: {input.Category}");

            return ObjectMapper.Map<Expense, ExpenseDto>(createdExpense);
        }

        public async Task<ExpenseDto> UpdateAsync(Guid id, CreateUpdateExpenseDto input)
        {
            Logger.LogInformation($"Update Expense requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Update Expense requested for ID: {id} with data: {input}");

            // Validate business rules
            if (string.IsNullOrWhiteSpace(input.Description))
            {
                throw new UserFriendlyException("Description is required.");
            }

            if (input.Amount <= 0)
            {
                throw new UserFriendlyException("Amount must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(input.Category))
            {
                throw new UserFriendlyException("Category is required.");
            }

            // Get the expense record
            var expense = await _expenseRepository.GetAsync(id);

            // Update the expense
            expense.Description = input.Description.Trim();
            expense.Amount = input.Amount;
            expense.ExpenseDate = input.ExpenseDate;
            expense.Category = input.Category.Trim();

            var updatedExpense = await _expenseRepository.UpdateAsync(expense);

            Logger.LogInformation($"Expense {id} updated successfully. New amount: {input.Amount}, Category: {input.Category}");

            return ObjectMapper.Map<Expense, ExpenseDto>(updatedExpense);
        }

        public async Task DeleteAsync(Guid id)
        {
            Logger.LogInformation($"Delete Expense requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Delete Expense requested for ID: {id}");

            await _expenseRepository.DeleteAsync(id);

            Logger.LogInformation($"Expense {id} deleted successfully.");
        }
    }
} 