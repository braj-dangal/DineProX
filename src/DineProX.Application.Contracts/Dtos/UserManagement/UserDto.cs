using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.UserManagement
{
    public class UserDto : AuditedEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
        public bool IsActive { get; set; }
    }
    public class RoleDto
    {
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
