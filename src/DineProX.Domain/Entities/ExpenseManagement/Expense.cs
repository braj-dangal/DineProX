using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.ExpenseManagement
{
    public class Expense : AuditedAggregateRoot<Guid>
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Category { get; set; }

        protected Expense()
        {
        }

        public Expense(Guid id, string description, decimal amount, DateTime expenseDate, string category)
            : base(id)
        {
            Description = description;
            Amount = amount;
            ExpenseDate = expenseDate;
            Category = category;
        }
    }
} 