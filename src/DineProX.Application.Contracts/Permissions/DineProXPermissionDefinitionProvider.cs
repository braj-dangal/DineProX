using DineProX.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DineProX.Permissions;

public class DineProXPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DineProXPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DineProXPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DineProXResource>(name);
    }
}
