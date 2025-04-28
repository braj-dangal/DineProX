namespace DineProX.Permissions;

public static class DineProXPermissions
{
    public const string GroupName = "DineProX";

    public const string GroupUser = "User";
    public const string GroupRole = "Role";

    public static class User
    {
        public const string Default = GroupUser + ".Default";
        public const string User_Create = GroupUser + ".Create";
        public const string User_Deactivate = GroupUser + ".Deactivate";
        public const string User_Edit = GroupUser + ".Edit";
    }

    public static class Role
    {
        public const string Default = GroupRole + ".Default";
        public const string Role_Create = GroupRole + ".Create";
        public const string Role_Deactivate = GroupRole + ".Deactivate";
        public const string Role_Edit = GroupRole + ".Edit";
    }
}
