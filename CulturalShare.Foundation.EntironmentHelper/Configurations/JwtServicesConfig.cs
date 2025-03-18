namespace CulturalShare.Foundation.EntironmentHelper.Configurations;

public class JwtServicesConfig
{
    public string Issuer { get; set; }
    public int SecondsUntilExpireUserJwtToken { get; set; }
    public int SecondsUntilExpireUserRefreshToken { get; set; }
    public int SecondsUntilExpireServiceJwtToken { get; set; }
    public Dictionary<string, string> JwtSecretTokenPairs { get; set; }
}
