using StackExchange.Redis;

namespace CulturalShare.Foundation.Authorization.JwtServices;

public class JwtBlacklistService : IJwtBlacklistService
{
    private const string BlacklistTokenPrefix = "jwt:blacklist:";
    private const string BlacklistUserPrefix = "user:blacklist:";

    private readonly IDatabase _redisDb;

    public JwtBlacklistService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    /// <summary>
    /// Checks if a JWT token is blacklisted.
    /// </summary>
    public async Task<bool> IsTokenBlacklistedAsync(string jti)
    {
        return await _redisDb.KeyExistsAsync(BlacklistTokenPrefix + jti);
    }

    /// <summary>
    /// Blacklists a JWT token until its expiration time.
    /// </summary>
    public async Task BlacklistTokenAsync(string jti, TimeSpan expiry)
    {
        await _redisDb.StringSetAsync(BlacklistTokenPrefix + jti, "revoked", expiry);
    }

    /// <summary>
    /// Removes a JWT token from the blacklist manually.
    /// </summary>
    public async Task RemoveFromBlacklistAsync(string jti)
    {
        await _redisDb.KeyDeleteAsync(BlacklistTokenPrefix + jti);
    }

    /// <summary>
    /// Adds a user ID to the user blacklist cache.
    /// </summary>
    public async Task BlacklistUserAsync(int userId, TimeSpan expiry)
    {
        await _redisDb.StringSetAsync(BlacklistUserPrefix + userId, "revoked", expiry);
    }

    /// <summary>
    /// Checks if a user is blacklisted.
    /// </summary>
    public async Task<bool> IsUserBlacklistedAsync(int userId)
    {
        return await _redisDb.KeyExistsAsync(BlacklistUserPrefix + userId);
    }

    /// <summary>
    /// Removes a user from the blacklist.
    /// </summary>
    public async Task RemoveUserFromBlacklistAsync(int userId)
    {
        await _redisDb.KeyDeleteAsync(BlacklistUserPrefix + userId);
    }
}
