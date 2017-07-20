using System;
using System.Text;
using NLog;
using Collectively.Common.Extensions;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Collectively.Common.Types;

namespace Collectively.Common.Security
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly TokenValidationParameters TokenValidationParameters;
        private readonly JwtTokenSettings _settings;
        private readonly SecurityKey _hmacSecretKey;
        private SecurityKey _rsaPrivateKey;
        private SecurityKey _rsaPublicKey;

        public JwtTokenHandler(JwtTokenSettings settings)
        {
            _settings = settings;
            if(_settings.UseRsa)
            {
                InitializeRsaKeys();
            }
            else
            {
                _hmacSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            }
            TokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuer = _settings.ValidateIssuer,
                IssuerSigningKey = _settings.UseRsa ? _rsaPublicKey : _hmacSecretKey
            }; 
        }

        public Maybe<JsonWebToken> Create(string userId, string role, TimeSpan? expiry = null)
        {
            var now = DateTime.UtcNow;
            var expires = expiry.HasValue ? 
                now.AddTicks(expiry.Value.Ticks) : 
                now.AddDays(_settings.ExpiryDays);
            
            return _settings.UseRsa ? 
                CreateTokenUsingRSA(userId, role, expires) : 
                CreateTokenUsingHmac(userId, role, expires);
        }

        private JsonWebToken CreateTokenUsingHmac(string userId, string role, DateTime expires)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                
            return CreateToken(userId, role, expires, signingCredentials);
        }

        private JsonWebToken CreateTokenUsingRSA(string userId, string role, DateTime expires)
        {
            using (RSA rsa = RSA.Create())
            {
                var privateKeyXml = _settings.UseRsaFilePath ? 
                    System.IO.File.ReadAllText(_settings.RsaPrivateKeyXML) :
                    _settings.RsaPrivateKeyXML;
                rsa.FromXmlString(privateKeyXml);
                var privateSecurityKey = new RsaSecurityKey(rsa);
                var signingCredentials = new SigningCredentials(privateSecurityKey, SecurityAlgorithms.RsaSha256);
                
                return CreateToken(userId, role, expires, signingCredentials);
            }
        }

        private void InitializeRsaKeys()
        {
            using(RSA publicRsa = RSA.Create())
            {
                var publicKeyXml = _settings.UseRsaFilePath ? 
                    System.IO.File.ReadAllText(_settings.RsaPublicKeyXML) :
                    _settings.RsaPublicKeyXML;
                publicRsa.FromXmlString(publicKeyXml);
                _rsaPublicKey = new RsaSecurityKey(publicRsa);
            }
            using(RSA privateRsa = RSA.Create())
            {
                var privateKeyXml = _settings.UseRsaFilePath ? 
                    System.IO.File.ReadAllText(_settings.RsaPrivateKeyXML) :
                    _settings.RsaPrivateKeyXML;
                privateRsa.FromXmlString(privateKeyXml);
                _rsaPrivateKey = new RsaSecurityKey(privateRsa);
            }
        }

        private JsonWebToken CreateToken(string userId, string role, DateTime expires,
            SigningCredentials signingCredentials)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };
            var jwt = new JwtSecurityToken(
                issuer: _settings.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
            );
            var token = JwtSecurityTokenHandler.WriteToken(jwt);

            return new JsonWebToken
            {
                Token = token,
                Expires = expires.ToTimestamp()
            };
        }

        public Maybe<string> GetFromAuthorizationHeader(string authorizationHeader)
        {
            var data = authorizationHeader.Trim().Split(' ');
            if (data.Length != 2)
            {
                return null;
            }
            if (data[0].Empty() || data[1].Empty())
            {
                return null;
            }
            var authorizationType = data[0].ToLowerInvariant();
            if (authorizationType != "bearer")
            {
                return null;
            }

            return data[1];
        }

        public Maybe<JsonWebToken> Parse(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedSecurityToken = null;
            try
            {
                handler.ValidateToken(token, TokenValidationParameters, out validatedSecurityToken);
                var validatedJwt = validatedSecurityToken as JwtSecurityToken;

                return new JsonWebToken
                {
                    Token = token,
                    Subject = validatedJwt.Subject,
                    Expires = validatedJwt.ValidTo.ToTimestamp()
                };
            }
            catch(Exception exception)
            {
                Logger.Error(exception, $"JWT Token parser error. {exception.Message}");

                return null;
            }   
        }
    }
}