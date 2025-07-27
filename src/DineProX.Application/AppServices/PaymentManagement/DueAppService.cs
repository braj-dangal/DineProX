using DineProX.Dtos.PaymentManagement;
using DineProX.Entities.PaymentManagement;
using DineProX.Interfaces.PaymentManagement;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace DineProX.AppServices.PaymentManagement
{
    public class DueAppService : ApplicationService, IDueAppService
    {
        private readonly IRepository<Due, Guid> _dueRepository;

        public DueAppService(IRepository<Due, Guid> dueRepository)
        {
            _dueRepository = dueRepository;
        }

        public async Task<DueDto> GetAsync(Guid id)
        {
            Logger.LogInformation($"Get Due requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Get Due requested for ID: {id}");

            var due = await _dueRepository.GetAsync(id);
            return ObjectMapper.Map<Due, DueDto>(due);
        }

        public async Task<List<DueDto>> GetUnsettledListAsync()
        {
            Logger.LogInformation($"Get Unsettled Due List requested by User: {CurrentUser.Id}");

            var unsettledDues = await _dueRepository.GetListAsync(d => !d.IsSettled);
            return ObjectMapper.Map<List<Due>, List<DueDto>>(unsettledDues);
        }

        public async Task<DueDto> SettleAsync(SettleDueDto input)
        {
            Logger.LogInformation($"Settle Due requested by User: {CurrentUser.Id}");
            Logger.LogDebug($"Settle Due requested for ID: {input.DueId} with amount: {input.AmountPaid}");

            // Validate input
            if (input.AmountPaid <= 0)
            {
                throw new UserFriendlyException("Amount paid must be greater than 0.");
            }

            // Get the due record
            var due = await _dueRepository.GetAsync(input.DueId);

            // Check if due is already settled
            if (due.IsSettled)
            {
                throw new UserFriendlyException("This due is already settled.");
            }

            // Check if payment amount exceeds remaining due
            if (input.AmountPaid > due.RemainingDue)
            {
                throw new UserFriendlyException($"Payment amount ({input.AmountPaid}) cannot exceed remaining due amount ({due.RemainingDue}).");
            }

            // Update the due record
            due.AmountPaid += input.AmountPaid;
            due.RemainingDue = due.TotalAmount - due.AmountPaid;

            // Check if due is now fully settled
            if (due.RemainingDue <= 0)
            {
                due.IsSettled = true;
                due.RemainingDue = 0; // Ensure it's exactly 0
                Logger.LogInformation($"Due {input.DueId} is now fully settled.");
            }

            // Save the updated due
            var updatedDue = await _dueRepository.UpdateAsync(due);

            Logger.LogInformation($"Due {input.DueId} settled with amount {input.AmountPaid}. Remaining due: {updatedDue.RemainingDue}");

            return ObjectMapper.Map<Due, DueDto>(updatedDue);
        }
    }
} 