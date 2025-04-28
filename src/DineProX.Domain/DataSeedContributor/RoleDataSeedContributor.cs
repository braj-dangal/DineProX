using DineProX.Constants.RoleManagement;
using DineProX.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Timing;

namespace DineProX.DataSeedContributor
{
    public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<IdentityRole, Guid> _rolesRepository;
        private readonly PermissionDefinitionManager _permissionDefinitionManager;
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IClock _clock;

        public RoleDataSeedContributor(IRepository<IdentityRole, Guid> rolesRepository,
                                       PermissionDefinitionManager permissionDefinitionManager,
                                       IPermissionGrantRepository permissionGrantRepository,
                                       IClock clock)
        {
            _rolesRepository = rolesRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
            _permissionGrantRepository = permissionGrantRepository;
            _clock = clock;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var roles = await _rolesRepository.GetListAsync();
            if (!roles.Any(x => x.Name.ToLower() == RoleConstants.General.ToLower()))
            {
                var providerName = "R";
                var defaultRole = new IdentityRole(new Guid("b67d7615-4a40-402f-8fad-ab2b8e42762c"), RoleConstants.General)
                {
                    IsPublic = true,
                };
                defaultRole.SetProperty("CreationTime", _clock.Now.ToString());
                await _rolesRepository.InsertAsync(defaultRole);
                var permissionList = await _permissionDefinitionManager.GetPermissionsAsync();
                var ManageRolePermissionSetting = permissionList.FirstOrDefault(x => x.Name.ToLower() == "AbpIdentity.Roles.ManagePermissions".ToLower());
                var PermissionsGrants = new List<PermissionGrant>()
                {
                    new PermissionGrant(new Guid("d67d7615-4a40-402f-8fad-ab2b8e42762d"), DineProXPermissions.User.Default, providerName, defaultRole.Name),
                    new PermissionGrant(new Guid("d67d7615-4a40-402f-8fad-ab2b8e42762e"), DineProXPermissions.Role.Default, providerName, defaultRole.Name),
                    new PermissionGrant(new Guid("e67d7615-4a40-402f-8fad-ab2b8e42762c"), ManageRolePermissionSetting.Name, providerName, defaultRole.Name)
                };
                await _permissionGrantRepository.InsertManyAsync(PermissionsGrants);
            }
        }
    }
}
