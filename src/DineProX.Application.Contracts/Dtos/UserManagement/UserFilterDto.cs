using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DineProX.Dtos.UserManagement
{
    public class UserFilterDto : PagedAndSortedResultRequestDto
    {
        public string? SortOrder { get; set; } = "asc";
        public string? SearchKeyword { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }

    }
}
