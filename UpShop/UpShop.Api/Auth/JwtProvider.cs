using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace UpShop.Api.Auth
{
    /// <summary>
    /// Provide a way to create a token based on global settings.
    /// </summary>
    public class JwtProvider
    {
        private JwtSecurityTokenHandler tokenHandler;
        private JwtSettings settings;

        public JwtProvider(JwtSettings settings, JwtSecurityTokenHandler tokenHandler)
        {
            this.tokenHandler = tokenHandler;
            this.settings = settings;
        }

        public string CreateEncoded(string userName)
        {
            string encoded = tokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new GenericIdentity(userName)),
                Expires = DateTime.UtcNow.AddDays(settings.TokenExpiration),
                SigningCredentials = new SigningCredentials(settings.SecurityKey, SecurityAlgorithms.HmacSha256Signature),
                Audience = settings.Audience,
                Issuer = settings.Issuer
            });

            return encoded;
        }

    }
}
