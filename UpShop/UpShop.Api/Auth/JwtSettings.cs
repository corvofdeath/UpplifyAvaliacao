using Microsoft.IdentityModel.Tokens;

namespace UpShop.Api.Auth
{
    /// <summary>
    ///  Global settings for all tokens.
    /// </summary>
    public class JwtSettings
    {
        public SymmetricSecurityKey SecurityKey { get; set; }
        public int TokenExpiration { get; }
        public string Audience { get; set; } = "DummyAudiency";
        public string Issuer { get; set; } = "DummyIssuer";

        public JwtSettings(SymmetricSecurityKey securityKey, int tokenExpiration)
        {
            SecurityKey = securityKey;
            TokenExpiration = tokenExpiration;
        }

    }
}
