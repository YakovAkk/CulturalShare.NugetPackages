namespace CulturalShare.Foundation.Authorization.AuthRoles;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Support = "Support";

    public const string AllRoles = User + "," + Admin + "," + Support;
}