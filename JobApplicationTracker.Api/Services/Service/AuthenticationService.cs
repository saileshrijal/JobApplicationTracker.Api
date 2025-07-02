using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Dto;
using JobApplicationTracker.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobApplicationTracker.Api.Services.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IOptions<JwtSettings> settings)
        {
            _jwtSettings = settings.Value;
        }
        public string GenerateJwtToken(UsersDto user)
        {
            var claims = new Claim[]
           {
                new Claim("email",user.Email.ToString()),
                new Claim("userId", user.UserId.ToString()),    
           };

            //  the custom key that we have set in the appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            //create signingCredentials and hash it with a algorithm.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a instance of JWTSecurityToken() for creating a token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_jwtSettings.ExpireMinutes)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

   
    }
}
