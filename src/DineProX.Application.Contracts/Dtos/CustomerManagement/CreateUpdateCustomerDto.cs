using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.CustomerManagement
{
    public class CreateUpdateCustomerDto : EntityDto<Guid?>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        public Guid? UserId { get; set; }
    }
} 