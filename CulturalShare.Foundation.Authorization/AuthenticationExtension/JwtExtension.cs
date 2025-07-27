using CulturalShare.Foundation.Authorization.JwtServices;
using CulturalShare.Foundation.EnvironmentHelper.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CulturalShare.Foundation.Authorization.AuthenticationExtension;

public static class JwtExtension
{
    public static void ConfigureJwtBearerOptions(JwtBearerOptions options, JwtServicesConfig jwtSettings)
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = GetTokenValidationParameters(jwtSettings);
        options.Events = GetJwtBearerEvents();
    }

    private static TokenValidationParameters GetTokenValidationParameters(JwtServicesConfig jwtSettings)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                ResolveIssuerSigningKey(token, jwtSettings),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            AudienceValidator = (audiences, securityToken, parameters) =>
                ValidateAudience(audiences, jwtSettings),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    private static SecurityKey[] ResolveIssuerSigningKey(string token, JwtServicesConfig jwtSettings)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadJwtToken(token);

        var serviceId = jwt.Audiences.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(serviceId))
        {
            throw new SecurityTokenException("Missing or invalid audience in token.");
        }

        var serviceConfig = jwtSettings.ServicesJwtConfigs
            .FirstOrDefault(cfg => cfg.ServiceId == serviceId);

        if (serviceConfig == null || string.IsNullOrWhiteSpace(serviceConfig.ServiceSecret))
        {
            throw new SecurityTokenException($"Invalid service configuration for audience '{serviceId}'.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(serviceConfig.ServiceSecret);
        var securityKey = new SymmetricSecurityKey(keyBytes);

        return new[] { securityKey };
    }

    private static bool ValidateAudience(IEnumerable<string> audiences, JwtServicesConfig jwtSettings)
    {
        var validServiceIds = jwtSettings.ServicesJwtConfigs.Select(x => x.ServiceId);
        return audiences.Any(a => validServiceIds.Contains(a));
    }

    private static JwtBearerEvents GetJwtBearerEvents()
    {
        return new JwtBearerEvents
        {
            OnTokenValidated = async context => await ValidateTokenAsync(context)
        };
    }

    private static async Task ValidateTokenAsync(TokenValidatedContext context)
    {
        var principal = context.Principal;
        if (principal == null)
        {
            context.Fail("Missing principal.");
            return;
        }

        var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

        var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IJwtBlacklistService>();

        if (!string.IsNullOrWhiteSpace(jti) && await blacklistService.IsTokenBlacklistedAsync(jti))
        {
            context.Fail("Token is revoked.");
            return;
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            context.Fail("Missing user ID claim.");
            return;
        }

        if (await blacklistService.IsUserBlacklistedAsync(userIdClaim))
        {
            context.Fail("User is revoked.");
            return;
        }
    }
}