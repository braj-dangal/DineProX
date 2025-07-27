using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.InventoryManagement
{
    public class CreatePurchaseDto : EntityDto<Guid?>
    {
        [Required]
        public Guid DishId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Supplier name cannot exceed 100 characters")]
        public string SupplierName { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
} 