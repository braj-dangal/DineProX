using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TISAPayrollManagement.Constants.RoleManagement;
using TISAPayrollManagement.Dtos.ResponseDtos;
using TISAPayrollManagement.Dtos.RoleManagement;
using TISAPayrollManagement.Entities.RoleManagement;
using TISAPayrollManagement.Interface.RoleManagement;
using TISAPayrollManagement.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace DineProX.Interfaces.RoleManagement
{
    [Authorize]
    public class RoleManagementAppService : ApplicationService, IRoleManagementAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IRepository<IdentityRole, Guid> _rolesRepository;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly PermissionDefinitionManager _permissionDefinitionManager;
        private readonly ILogger<RoleManagementAppService> _logger;
        private readonly IRepository<RoleExtension, Guid> _roleExtensionRepository;
        protected readonly IDistributedCache<PermissionGrantCacheItem> Cache;

        public RoleManagementAppService(IPermissionGrantRepository permissionGrantRepository,
            IdentityRoleManager roleManager,
            IRepository<RoleExtension, Guid> roleExtensionRepository,
            PermissionDefinitionManager permissionDefinitionManager,
            IRepository<IdentityRole, Guid> rolesRepository,
            IdentityUserManager userManager,
            IRepository<IdentityUser, Guid> identityUserRepository,
            IDistributedCache<PermissionGrantCacheItem> cache,
            ILogger<RoleManagementAppService> logger)
        {
            _permissionGrantRepository = permissionGrantRepository;
            _roleManager = roleManager;
            _roleExtensionRepository = roleExtensionRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
            _rolesRepository = rolesRepository;
            _userManager = userManager;
            _identityUserRepository = identityUserRepository;
            Cache = cache;
            _logger = logger;
        }

        [Authorize(TISAPayrollManagementPermissions.Role.Role_Create)]
        public async Task<ResponseDto<GetRolesDto>> CreateRoleAsync(CreateRoleDto input)
        {
            try
            {
                _logger.LogInformation($"CreateRoleAsync requested by User: {CurrentUser.Id}");

                var listOfPermissions = new List<string>();
                var providerName = "R";

                var roleList = await _rolesRepository.GetListAsync();
                var checkDuplicateRole = roleList.Any(x => x.Name.Trim().ToLower() == input.RoleName.Trim().ToLower());

                if (checkDuplicateRole)
                {
                    throw new UserFriendlyException("Role with same name already exists.", code: "400");
                }

                if (input.Permissions.Count() == 0)
                {
                    throw new UserFriendlyException("Atleast One permission is required", code: "400");
                }

                var newRole = new IdentityRole(GuidGenerator.Create(), input.RoleName.Trim())
                {
                    IsPublic = true,
                };
                newRole.SetProperty("CreationTime", DateTime.UtcNow.ToString());

                var insertedRole = await _rolesRepository.InsertAsync(newRole);

                var roleExtension = new RoleExtension()
                {
                    AbpRoleId = insertedRole.Id,
                    AbpRoleName = insertedRole.Name.Trim(),
                    Description = input.Description.Trim(),
                    IsActive = true
                };
                await _roleExtensionRepository.InsertAsync(roleExtension);

                var permissionsLists = await _permissionDefinitionManager.GetPermissionsAsync();
                var permissionGrants = await _permissionGrantRepository.GetListAsync();
                var PermissionsGrants = new List<PermissionGrant>();
                input.Permissions.Add(PermissionConstants.Role);
                foreach (var permission in input.Permissions)
                {
                    var permissionNames = permissionsLists.Select(x => x.Name.ToLower());
                    if (!permissionNames.Contains(permission.ToLower()))
                    {
                        throw new UserFriendlyException($"{permission} not found");
                    }

                    var isPermissionInRole = permissionGrants.Any(x => x.Name.Equals(permission) && x.ProviderKey.Equals(newRole.Name) && x.ProviderName == "R");
                    if (!isPermissionInRole)
                    {
                        var permissionEntity = new PermissionGrant(GuidGenerator.Create(), permission, providerName, newRole.Name);
                        PermissionsGrants.Add(permissionEntity);
                        listOfPermissions.Add(permissionEntity.Name);
                    }
                }

                await _permissionGrantRepository.InsertManyAsync(PermissionsGrants);
                var result = new GetRolesDto()
                {
                    RoleId = insertedRole.Id,
                    RoleName = insertedRole.Name,
                    Permissions = listOfPermissions,
                    isActive = roleExtension.IsActive
                };
                var response = new ResponseDto<GetRolesDto>
                {
                    Message = "Role added Successfully",
                    Code = 200,
                    Success = true,
                    Data = result
                };
                _logger.LogInformation($"CreateRoleAsync completed by User: {CurrentUser.Id}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new UserFriendlyException(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(TISAPayrollManagementPermissions.Role.Default)]
        public async Task<PagedResultDto<GetAllRolesDto>> GetPagedAndSortedRoleListAsync(SerachDto input)
        {
            try
            {
                _logger.LogInformation($"GetPagedAndSortedRoleListAsync requested by User: {CurrentUser.Id}");

                var roles = await _rolesRepository.GetQueryableAsync();
                var roleExtensions = await _roleExtensionRepository.GetQueryableAsync();
                var permissions = await _permissionGrantRepository.GetListAsync();
                var finalRoleList = from role in roles
                                    join roleExt in roleExtensions on role.Id equals roleExt.AbpRoleId into r
                                    from re in r.DefaultIfEmpty()
                                    where role.Name.ToLower() != RoleConstants.SuperAdmin.ToLower()
                                          && role.Name.ToLower() != RoleConstants.Admin.ToLower()
                                    select new GetAllRolesDto()
                                    {
                                        RoleId = role.Id,
                                        RoleName = role.Name,
                                        Description = re != null ? re.Description : string.Empty,
                                        isActive = re != null ? re.IsActive : true,
                                    };
                if (finalRoleList == null)
                {
                    throw new UserFriendlyException("No Role Found");
                }

                bool isSearched = input.Search.IsNullOrEmpty();

                if (!isSearched)
                {
                    finalRoleList = finalRoleList.Where(x => x.RoleName != null && x.RoleName.ToLower().Contains(input.Search.ToLower())
                                                             || x.Description != null && x.Description.ToLower().Contains(input.Search.ToLower()));
                }

                if (input.isActive != null)
                {
                    if (input.isActive.ToLower() == "true")
                    {
                        finalRoleList = finalRoleList.Where(x => x.isActive == true);
                    }
                    else if (input.isActive.ToLower() == "false")
                    {
                        finalRoleList = finalRoleList.Where(x => x.isActive == false);
                    }
                    else
                    {
                        throw new UserFriendlyException("No Active status Found");
                    }
                }

                var totalCount = finalRoleList.Count();
                var result = new List<GetAllRolesDto>();
                if (input.Sorting.IsNullOrWhiteSpace())
                {
                    input.Sorting = "RoleName";
                }

                switch (input.Sorting.ToLower())
                {
                    case "description":
                        result = input.SortType == "asc"
                            ? finalRoleList
                                .AsEnumerable()
                                .OrderBy(r => r.Description, StringComparer.OrdinalIgnoreCase)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList()
                            : finalRoleList
                                .AsEnumerable()
                                .OrderByDescending(r => r.Description, StringComparer.OrdinalIgnoreCase)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList();
                        break;
                    default:
                        result = input.SortType == "asc"
                            ? finalRoleList
                                .AsEnumerable()
                                .OrderBy(r => r.RoleName, StringComparer.OrdinalIgnoreCase)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList()
                            : finalRoleList
                                .AsEnumerable()
                                .OrderByDescending(r => r.RoleName, StringComparer.OrdinalIgnoreCase)
                                .Skip(input.SkipCount)
                                .Take(input.MaxResultCount)
                                .ToList();
                        break;
                }

                _logger.LogInformation($"GetPagedAndSortedRoleListAsync completed by User: {CurrentUser.Id}");
                return new PagedResultDto<GetAllRolesDto>(totalCount, result.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(TISAPayrollManagementPermissions.Role.Default)]
        public async Task<GetRolesDto> GetRoleByIdAsync(Guid Id)
        {
            try
            {
                _logger.LogInformation($"GetRoleByIdAsync requested by User: {CurrentUser.Id}");

                var providerName = "R";

                var roleCheck = await _rolesRepository.FindAsync(Id);
                var roleExtension = await _roleExtensionRepository.FirstOrDefaultAsync(x => x.AbpRoleId == Id && x.IsActive == true);
                if (roleCheck == null)
                {
                    throw new UserFriendlyException("No Role Found");
                }
                else
                {
                    var role = new GetRolesDto()
                    {
                        RoleId = roleCheck.Id,
                        RoleName = roleCheck.Name,
                        Description = roleExtension == null ? null : roleExtension.Description,
                        isActive = roleExtension == null ? null : roleExtension.IsActive,
                    };
                    var permissionGrants = (await _permissionGrantRepository.GetListAsync()).Where(x => x.ProviderKey == role.RoleName
                                                                                                        && x.ProviderName == providerName).Select(x => x.Name).ToList();
                    role.Permissions = permissionGrants;
                    _logger.LogInformation($"GetRoleByIdAsync completed by User: {CurrentUser.Id}");
                    return role;
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(TISAPayrollManagementPermissions.Role.Role_Edit)]
        public async Task<ResponseDto<GetRolesDto>> UpdateRoleAsync(Guid Id, CreateRoleDto input)
        {
            try
            {
                _logger.LogInformation($"UpdateRoleAsync requested by User: {CurrentUser.Id}");
                var providerName = "R";
                var listOfPermissions = new List<string>();
                if (input.Permissions.Count() == 0)
                {
                    throw new UserFriendlyException("Permission should be added in a role", code: "400");
                }

                var roleCheck = await _roleManager.GetByIdAsync(Id);
                var roleList = await _rolesRepository.GetListAsync();
                var oldRoleName = roleCheck.Name;
                var checkDuplicateRole = roleList.Any(x => x.Name.Trim().ToLower() == input.RoleName.Trim().ToLower() && x.Id != Id);

                if (checkDuplicateRole)
                {
                    throw new UserFriendlyException("Role with same name already exists.", code: "400");
                }

                if (roleCheck == null)
                {
                    throw new UserFriendlyException("No Role Found");
                }
                else if (roleCheck.Name.ToLower() == "admin" || roleCheck.Name.ToLower() == RoleConstants.General.ToLower())
                {
                    throw new UserFriendlyException("Cannot Mofidy Admin or General User Role");
                }

                else
                {
                    await _roleManager.SetRoleNameAsync(roleCheck, input.RoleName.Trim());
                    await _roleManager.UpdateAsync(roleCheck);

                    var permissionList = (await _permissionGrantRepository.GetListAsync())
                        .Where(x => x.ProviderKey == oldRoleName && x.ProviderName == providerName);

                    var caches = permissionList.Select(x => new KeyValuePair<string, PermissionGrantCacheItem>(
                        PermissionGrantCacheItem.CalculateCacheKey(x.Name, x.ProviderName, x.ProviderKey),
                        new PermissionGrantCacheItem(true))).ToList();

                    await _permissionGrantRepository.DeleteManyAsync(permissionList, true);
                    var permissionGrantCaches = await Cache.GetManyAsync(caches.Select(x => x.Key));
                    if (permissionGrantCaches.Any())
                    {
                        await Cache.RemoveManyAsync(permissionGrantCaches.Select(x => x.Key));
                    }

                    var existedRoleExtension = await _roleExtensionRepository.FirstOrDefaultAsync(x => x.AbpRoleId == Id);
                    if (existedRoleExtension != null)
                    {
                        existedRoleExtension.AbpRoleName = input.RoleName.Trim();
                        existedRoleExtension.Description = input.Description.Trim();
                        await _roleExtensionRepository.UpdateAsync(existedRoleExtension);
                    }

                    var permissionsLists = await _permissionDefinitionManager.GetPermissionsAsync();
                    var permissionGrants = await _permissionGrantRepository.GetListAsync();
                    var PermissionsGrants = new List<PermissionGrant>();
                    input.Permissions.Add(PermissionConstants.Role);
                    foreach (var permission in input.Permissions)
                    {
                        var permissionNames = permissionsLists.Select(x => x.Name.ToLower());
                        if (!permissionNames.Contains(permission.ToLower()))
                        {
                            throw new UserFriendlyException($"{permission} not found");
                        }

                        var isPermissionInRole = permissionGrants.Any(x => x.Name.Equals(permission) && x.ProviderKey.Equals(roleCheck.Name) && x.ProviderName == "R");
                        if (!isPermissionInRole)
                        {
                            var permissionEntity = new PermissionGrant(GuidGenerator.Create(), permission, providerName, roleCheck.Name);
                            PermissionsGrants.Add(permissionEntity);
                            listOfPermissions.Add(permissionEntity.Name);
                        }

                        var cachePermission = PermissionsGrants.Select(x => new KeyValuePair<string, PermissionGrantCacheItem>(
                            PermissionGrantCacheItem.CalculateCacheKey(x.Name, x.ProviderName, x.ProviderKey),
                            new PermissionGrantCacheItem(true))).ToList();
                        await Cache.SetManyAsync(cachePermission);
                    }

                    await _permissionGrantRepository.InsertManyAsync(PermissionsGrants);
                    var result = new GetRolesDto()
                    {
                        RoleId = roleCheck.Id,
                        RoleName = roleCheck.Name,
                        Permissions = listOfPermissions
                    };
                    var response = new ResponseDto<GetRolesDto>
                    {
                        Message = "Role Updated Successfully",
                        Code = 200,
                        Success = true,
                        Data = result
                    };
                    _logger.LogInformation($"UpdateRoleAsync completed by User: {CurrentUser.Id}");
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<RoleResponseDto>> GetRoleListAsync()
        {
            try
            {
                var roleQuery = await _rolesRepository.GetQueryableAsync();
                var roleExtensions = await _roleExtensionRepository.GetQueryableAsync();

                var generalUser = roleQuery.Where(x => x.Name.ToLower() == RoleConstants.General.ToLower()).FirstOrDefault();
                var query = (from role in roleQuery
                             join roleExtension in roleExtensions
                                 on role.Id equals roleExtension.AbpRoleId into r
                             from re in r.DefaultIfEmpty()
                             where (re == null || re.IsActive != null && re.IsActive == true)
                                   && role.Name.ToLower() != RoleConstants.SuperAdmin.ToLower()
                                   && role.Name.ToLower() != RoleConstants.Admin.ToLower()
                             select new RoleResponseDto()
                             {
                                 RoleId = role.Id,
                                 RoleName = role.Name,
                             }).ToList();
                return query;
            }
            catch (Exception)
            {
                Logger.LogError(nameof(GetRoleListAsync));
                throw;
            }
        }

        [Authorize(TISAPayrollManagementPermissions.Role.Role_Deactivate)]
        public async Task<ResponseDto<GetRolesDto>> UpdateRoleStatusAsync(StatusDto input)
        {
            try
            {
                _logger.LogInformation($"UpdateRoleStatusAsync requested by User: {CurrentUser.Id}");

                var roleExtension = await _roleExtensionRepository.FirstOrDefaultAsync(x => x.AbpRoleId == input.roleId);
                var role = await _rolesRepository.FirstOrDefaultAsync(x => x.Id == input.roleId);
                var usersInRoles = await _userManager.GetUsersInRoleAsync(role.Name);

                var validationResponse = new ResponseDto<GetRolesDto>();
                if (role == null)
                {
                    throw new UserFriendlyException("No Role Found");
                }
                else if (role.Name.ToLower() == RoleConstants.Admin.ToLower())
                {
                    throw new UserFriendlyException("Cannot Deactivate Admin Role");
                }
                else if (role.Name.ToLower() == RoleConstants.General.ToLower())
                {
                    throw new UserFriendlyException("Cannot Deactivate General User Role");
                }

                if (roleExtension == null)
                {
                    throw new UserFriendlyException("No Role Found");
                }

                if (input.isActive == false && input.isRoleDeactivateConfirmed == false)
                {
                    if (usersInRoles.Count != 0)
                    {
                        validationResponse.Message = "This role is already in use," +
                                                     " deactivating it will remove this role from all users.\n Are you sure you want to deactivate?";
                        return validationResponse;
                    }
                    else
                    {
                        validationResponse.Message = "Are you sure you want to deactivate?";
                        return validationResponse;
                    }
                }
                else if (input.isActive == false && input.isRoleDeactivateConfirmed == true)
                {
                    //Removing Roles From Users
                    var appUsers = await _identityUserRepository.GetListAsync();
                    foreach (var usersInRole in usersInRoles)
                    {
                        var user = appUsers.FirstOrDefault(x => x.UserName == usersInRole.UserName);
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }

                roleExtension.IsActive = input.isActive;
                await _roleExtensionRepository.UpdateAsync(roleExtension);

                var permissionGrants = (await _permissionGrantRepository.GetListAsync())
                    .Where(x => x.ProviderKey == role.Name
                                && x.ProviderName == "R").Select(x => x.Name).ToList();
                var roleResult = new ResponseDto<GetRolesDto>()
                {
                    Message = "Role Deactivated Successfully",
                    Code = 200,
                    Success = true,
                    Data = new GetRolesDto()
                    {
                        RoleId = input.roleId,
                        RoleName = role.Name,
                        Permissions = permissionGrants,
                        isActive = roleExtension.IsActive
                    }
                };
                _logger.LogInformation($"UpdateRoleStatusAsync completed by User: {CurrentUser.Id}");
                return roleResult;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}