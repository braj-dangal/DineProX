using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.PaymentManagement
{
    public class CreateUpdatePaymentDto : EntityDto<Guid?>
    {
        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount paid must be greater than or equal to 0")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
        public decimal Discount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total bill must be greater than or equal to 0")]
        public decimal TotalBill { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
} 