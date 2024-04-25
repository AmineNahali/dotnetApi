namespace backend.Helper;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtHelper()
{
    private readonly string _key = "5erg25ger5g483654rgeofihjgqeer65g4oiè2er6h5df8g8g64er65poàè";
    private readonly string _issuer = "your_issuer";
    private readonly string _audience = "your_audience";

    public string GenerateAccessToken(string username, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(_issuer, _audience, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        // Implement your logic for generating a refresh token
        // This could involve creating a new token with a longer expiration time
        // and storing it in a database associated with the user
        return Guid.NewGuid().ToString();
    }

    public string RefreshAccessToken(string refreshToken)
    {
        // Implement your logic for refreshing an access token
        // This could involve validating the refresh token, generating a new access token,
        // and updating the refresh token in the database
        return GenerateAccessToken("username", "role");
    }
}