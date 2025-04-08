using CulturalShare.Foundation.Authorization.JwtServices;
using CulturalShare.Foundation.EntironmentHelper.Configurations;
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

        if (string.IsNullOrEmpty(serviceId) || !jwtSettings.JwtSecretTokenPairs.TryGetValue(serviceId, out var serviceSecret))
        {
            throw new SecurityTokenException("Invalid audience");
        }

        return new[] { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serviceSecret)) };
    }

    private static bool ValidateAudience(IEnumerable<string> audiences, JwtServicesConfig jwtSettings)
    {
        var isAudienceValid = audiences.Any(a => jwtSettings.JwtSecretTokenPairs.ContainsKey(a));
        return isAudienceValid;
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
        var jti = principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IJwtBlacklistService>();

        if (!string.IsNullOrEmpty(jti) && await blacklistService.IsTokenBlacklistedAsync(jti))
        {
            context.Fail("Token is revoked.");
            return;
        }

        if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            context.Fail("Invalid user ID claim.");
            return;
        }

        if (await blacklistService.IsUserBlacklistedAsync(userId))
        {
            context.Fail("User is revoked.");
        }
    }
}
