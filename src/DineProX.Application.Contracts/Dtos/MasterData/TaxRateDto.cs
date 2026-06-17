using System;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.MasterData
{
    public class TaxRateDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTaxRateDto
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateTaxRateDto
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
