using DineProX.Dtos.ResponseDtos;
using DineProX.Dtos.UserManagement;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DineProX.Interfaces.UserManagement
{
    public interface IUserManagementAppService : IApplicationService
    {
        Task<ResponseDto<UserDto>> CreateUserAsync(CreateUserDto input);
        Task<PagedResultDto<UserDto>> GetPagedAndSortedUserListAsync(UserFilterDto input);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<ResponseDto<UserDto>> UpdateUserAsync(Guid id, UpdateUserDto input);
        Task<DeleteResponseDto> ActivateDeactivateUserAsync(ActivateDeactivateUserDto input);
    }
}
