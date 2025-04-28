using DineProX.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DineProX.Permissions;

public class DineProXPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var DineProX = context.AddGroup(DineProXPermissions.GroupName);


        var userPermission = DineProX.AddPermission(DineProXPermissions.User.Default, L("Permission:User"));
        userPermission.AddChild(DineProXPermissions.User.User_Create, L("Permission:User.Create"));
        userPermission.AddChild(DineProXPermissions.User.User_Deactivate, L("Permission:User.Deactivate"));
        userPermission.AddChild(DineProXPermissions.User.User_Edit, L("Permission:User.Edit"));

        var rolePermission = DineProX.AddPermission(DineProXPermissions.Role.Default, L("Permission:Role"));
        rolePermission.AddChild(DineProXPermissions.Role.Role_Create, L("Permission:Role.Create"));
        rolePermission.AddChild(DineProXPermissions.Role.Role_Deactivate, L("Permission:Role.Deactivate"));
        rolePermission.AddChild(DineProXPermissions.Role.Role_Edit, L("Permission:Role.Edit"));
        
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DineProXPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DineProXResource>(name);
    }
}
