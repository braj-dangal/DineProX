using DineProX.Dtos.ResponseDtos;
using DineProX.Dtos.RoleManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.RoleManagement
{
    public interface IRoleManagementAppService : IApplicationService
    {
        Task<ResponseDto<GetRolesDto>> CreateRoleAsync(CreateRoleDto input);
        Task<GetRolesDto> GetRoleByIdAsync(Guid Id);
        Task<ResponseDto<GetRolesDto>> UpdateRoleAsync(Guid Id, CreateRoleDto input);
        Task<PagedResultDto<GetAllRolesDto>> GetPagedAndSortedRoleListAsync(SerachDto input);
        Task<List<RoleResponseDto>> GetRoleListAsync();
        Task<ResponseDto<GetRolesDto>> UpdateRoleStatusAsync(StatusDto input);

    }
}
