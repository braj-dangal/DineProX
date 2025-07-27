using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.ExpenseManagement
{
    public class CreateUpdateExpenseDto : EntityDto<Guid?>
    {
        [Required]
        [StringLength(256, ErrorMessage = "Description cannot exceed 256 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        public string Category { get; set; }
    }
} 