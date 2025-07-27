using System;
using System.ComponentModel.DataAnnotations;

namespace DineProX.Dtos.PaymentManagement
{
    public class SettleDueDto
    {
        [Required]
        public Guid DueId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than 0")]
        public decimal AmountPaid { get; set; }
    }
} 