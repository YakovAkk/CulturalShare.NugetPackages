namespace CulturalShare.Foundation.EnvironmentHelper.Configurations;

public class JwtServicesConfig
{
    public string CurrentServiceId { get; set; }
    public string Issuer { get; set; }
    public int SecondsUntilExpireUserJwtToken { get; set; }
    public int SecondsUntilExpireUserRefreshToken { get; set; }
    public int SecondsUntilExpireServiceJwtToken { get; set; }
    public List<ServiceJwtConfig> ServicesJwtConfigs { get; set; }
}

public class ServiceJwtConfig
{
    public string ServiceSecret { get; set; }
    public string ServiceRole { get; set; }
    public string ServiceId { get; set; }
}
