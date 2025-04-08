namespace CulturalShare.Foundation.Authorization.JwtServices;

public interface IJwtBlacklistService
{
    Task<bool> IsTokenBlacklistedAsync(string jti);
    Task BlacklistTokenAsync(string jti, TimeSpan expiry);
    Task RemoveFromBlacklistAsync(string jti);
    Task BlacklistUserAsync(int userId, TimeSpan expiry);
    Task<bool> IsUserBlacklistedAsync(int userId);
    Task RemoveUserFromBlacklistAsync(int userId);
}

