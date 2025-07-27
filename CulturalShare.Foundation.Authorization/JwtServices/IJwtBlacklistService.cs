namespace CulturalShare.Foundation.Authorization.JwtServices;

public interface IJwtBlacklistService
{
    Task<bool> IsTokenBlacklistedAsync(string jti);
    Task BlacklistTokenAsync(string jti, TimeSpan expiry);
    Task RemoveFromBlacklistAsync(string jti);
    Task BlacklistUserAsync(string userId, TimeSpan expiry);
    Task<bool> IsUserBlacklistedAsync(string userId);
    Task RemoveUserFromBlacklistAsync(string userId);
}

