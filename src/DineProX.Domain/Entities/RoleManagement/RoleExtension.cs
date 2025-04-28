using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace DineProX.Entities.RoleManagement
{
    public class RoleExtension : FullAuditedAggregateRoot<Guid>
    {
        public Guid AbpRoleId { get; set; }
        public string AbpRoleName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
