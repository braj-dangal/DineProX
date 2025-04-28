using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.RoleManagement
{
    public class RolesDto
    {

    }
    public class GetRolesDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }
        public string? Description { get; set; }
        public bool? isActive { get; set; }
    }
    public class GetAllRolesDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public List<string> Permissions { get; set; }
        public bool? isActive { get; set; }
    }
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role is required")]
        [MaxLength(32)]
        public string RoleName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
        public List<string> Permissions { get; set; }
        public bool isActive { get; set; }
    }
    public class SerachDto : PagedAndSortedResultRequestDto
    {
        public string? Search { get; set; }
        public string? SortType { get; set; } = "asc";
        public string? isActive { get; set; }
    }
    public class StatusDto
    {
        public Guid roleId { get; set; }
        public bool isActive { get; set; }
        public bool isRoleDeactivateConfirmed { get; set; }
    }
}
