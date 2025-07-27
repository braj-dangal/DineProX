using System;
using System.ComponentModel.DataAnnotations;

namespace DineProX.Dtos.InventoryManagement
{
    public class UpdateInventoryDto
    {
        [Required]
        public Guid InventoryId { get; set; }

        [Required]
        public int QuantityChange { get; set; }
    }
} 