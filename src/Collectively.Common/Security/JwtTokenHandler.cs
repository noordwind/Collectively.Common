using System;
using System.Text;
using NLog;
using Collectively.Common.Extensions;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Collectively.Common.Types;
using System.Linq;

namespace Collectively.Common.Security
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JwtTokenSettings _settings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private TokenValidationParameters _tokenValidationParameters;
        private SecurityKey _issuerSigningKey;
        private SigningCredentials _signingCredentials;
        private JwtHeader _jwtHeader;

        public JwtTokenHandler(JwtTokenSettings settings)
        {
            _settings = settings;
            if(_settings.UseRsa)
            {
                InitializeRsa();
            }
            else
            {
                InitializeHmac();
            }
            InitializeJwtParameters();
        }

        public Maybe<JwtBasic> Create(string userId, string role, TimeSpan? expiry = null)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = (expiry.HasValue ? 
                nowUtc.AddTicks(expiry.Value.Ticks) : 
                nowUtc.AddDays(_settings.ExpiryDays));
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var issuer = _settings.Issuer ?? string.Empty;
            var payload = new JwtPayload
            {
                {"sub", userId},
                {"iss", issuer},
                {"iat", now},
                {"nbf", now},
                {"exp", exp},
                {"jti", Guid.NewGuid().ToString("N")}
            };
            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JwtBasic
            {
                Token = token,
                Expires = exp
            };
        }

        private void InitializeRsa()
        {
            using(RSA publicRsa = RSA.Create())
            {
                var publicKeyXml = _settings.UseRsaFilePath ? 
                    System.IO.File.ReadAllText(_settings.RsaPublicKeyXML) :
                    _settings.RsaPublicKeyXML;
                publicRsa.FromXmlString(publicKeyXml);
                _issuerSigningKey = new RsaSecurityKey(publicRsa);
            }
            using(RSA privateRsa = RSA.Create())
            {
                var privateKeyXml = _settings.UseRsaFilePath ? 
                    System.IO.File.ReadAllText(_settings.RsaPrivateKeyXML) :
                    _settings.RsaPrivateKeyXML;
                privateRsa.FromXmlString(privateKeyXml);
                var privateKey = new RsaSecurityKey(privateRsa);
                _signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
            }
        }

        private void InitializeHmac()
        {
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256); 
        }
        
        private void InitializeJwtParameters()
        {
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _settings.Issuer,
                ValidateIssuer = _settings.ValidateIssuer,
                IssuerSigningKey = _issuerSigningKey
            }; 
        }

        public Maybe<string> GetFromAuthorizationHeader(string authorizationHeader)
        {
            if(authorizationHeader.Empty())
            {
                return null;
            }
            var data = authorizationHeader.Trim().Split(' ');
            if (data.Length != 2 || data.Any(x => x.Empty()))
            {
                return null;
            }
            if (data[0].ToLowerInvariant() != "bearer")
            {
                return null;
            }

            return data[1];
        }

        public Maybe<JwtDetails> Parse(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedSecurityToken = null;
            try
            {
                handler.ValidateToken(token, _tokenValidationParameters, out validatedSecurityToken);
                var validatedJwt = validatedSecurityToken as JwtSecurityToken;

                return new JwtDetails
                {
                    Subject = validatedJwt.Subject,
                    Claims = validatedJwt.Claims,
                    Expires = validatedJwt.ValidTo.ToTimestamp(),
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