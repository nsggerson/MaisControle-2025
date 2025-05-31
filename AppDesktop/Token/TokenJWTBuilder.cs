using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AppDesktop.Token
{
    public class TokenJWTBuilder
    {
        private SecurityKey securityKey = null!;
        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 5;


        public TokenJWTBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public TokenJWTBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public TokenJWTBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public TokenJWTBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public TokenJWTBuilder AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public TokenJWTBuilder AddClaims(Dictionary<string, string> claims)
        {
            this.claims.Union(claims);
            return this;
        }

        public TokenJWTBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }


        private void EnsureArguments()
        {
            if (this.securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this.subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(this.issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this.audience))
                throw new ArgumentNullException("Audience");
        }

        public TokenJWT Builder()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,this.subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: new SigningCredentials(
                                                   this.securityKey,
                                                   SecurityAlgorithms.HmacSha256)

                );

            return new TokenJWT(token);

        }

    }
    //public class TokenJWTBuilder
    //{
    //    private SecurityKey _securityKey = null!; 
    //    private string _subject = string.Empty; 
    //    private string _issuer = string.Empty; 
    //    private string _audience = string.Empty; 
    //    private string _token = string.Empty;
    //    private Dictionary<string, string> _claims = new Dictionary<string, string>(); 
    //    private int _expiryInMinutes = 5; 

    //    public TokenJWTBuilder AddSecurityKey(SecurityKey securityKey)
    //    {
    //        this._securityKey = securityKey;
    //        return this;
    //    }

    //    public TokenJWTBuilder AddSubject(string subject)
    //    {
    //        this._subject = subject;
    //        return this;
    //    }

    //    public TokenJWTBuilder AddIssuer(string issuer)
    //    {
    //        this._issuer = issuer;
    //        return this;
    //    }

    //    public TokenJWTBuilder AddAudience(string audience)
    //    {
    //        this._audience = audience;
    //        return this;
    //    }

    //    public TokenJWTBuilder AddClaim(string type, string value)
    //    {
    //       this._claims.Add(type, value);
    //        return this;
    //    }

    //    public TokenJWTBuilder AddClaims(Dictionary<string, string> claims)
    //    {            
    //        this._claims.Union(claims);
    //        return this;
    //    }

    //    public TokenJWTBuilder AddExpiry(int expiryInMinutes)
    //    {
    //        this._expiryInMinutes = expiryInMinutes;
    //        return this;
    //    }

    //    public TokenJWTBuilder AddToken(string token)
    //    {
    //        this._token = token;
    //        return this;
    //    }

    //    private void EnsureArguments()
    //    {
    //        if (this._securityKey == null)
    //            throw new ArgumentNullException(nameof(this._securityKey));
    //        if (string.IsNullOrEmpty(this._subject))
    //            throw new ArgumentNullException(nameof(this._subject));
    //        if (string.IsNullOrEmpty(this._issuer))
    //            throw new ArgumentNullException(nameof(this._issuer));
    //        if (string.IsNullOrEmpty(this._audience))
    //            throw new ArgumentNullException(nameof(this._audience));
    //    }

    //    public TokenJWT Builder()
    //    {
    //        EnsureArguments();

    //        var claims = new List<Claim>
    //        {
    //            new Claim(JwtRegisteredClaimNames.Sub, this._subject),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //        }.Union(this._claims.Select(item => new Claim(item.Key, item.Value)));

    //        var token = new JwtSecurityToken(
    //            issuer: this._issuer,
    //            audience: this._audience,
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddMinutes(_expiryInMinutes),
    //            signingCredentials: new SigningCredentials(
    //                this._securityKey,
    //                SecurityAlgorithms.HmacSha256
    //            )
    //        );

    //        return new TokenJWT(token);
    //    }

    //    private void EnsureArgumentsValid()
    //    {
    //        if (string.IsNullOrEmpty(this._token))
    //            throw new ArgumentNullException(nameof(this._token));
    //        if (this._securityKey == null)
    //            throw new ArgumentNullException(nameof(this._securityKey));
    //        if (string.IsNullOrEmpty(this._issuer))
    //            throw new ArgumentNullException(nameof(this._issuer));
    //        if (string.IsNullOrEmpty(this._audience))
    //            throw new ArgumentNullException(nameof(this._audience));
    //    }

    //    public bool IsTokenValid()
    //    {
    //        EnsureArgumentsValid();
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var validationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidateIssuerSigningKey = true,
    //            ValidIssuer = this._issuer,
    //            ValidAudience = this._audience,
    //            IssuerSigningKey = this._securityKey
    //        };

    //        try
    //        {
    //            tokenHandler.ValidateToken(this._token, validationParameters, out SecurityToken validatedToken);
    //            return validatedToken != null;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //}
}
