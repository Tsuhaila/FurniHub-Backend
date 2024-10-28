using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniHub.Services.JwtServices
{
    public class JwtService:IJwtService
    {
        private readonly string secretKey;
        public JwtService(IConfiguration configuration)
        {
            secretKey = configuration["Jwt:Key"];
            
        }
        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler=new JwtSecurityTokenHandler();
                var securityKey=Encoding.UTF8.GetBytes(secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                var principal=tokenHandler.ValidateToken(token, validationParameters,out var validateToken);
                if(validateToken is not JwtSecurityToken jwtToken)
                {
                    throw new Exception("Invalid jwt token");
                }
                var userIdClaim=jwtToken.Claims.FirstOrDefault(claim=>claim.Type==ClaimTypes.NameIdentifier);
                if(userIdClaim==null||!int.TryParse(userIdClaim.Value,out var Id))
                {
                    throw new Exception("invalid or missing user Id claim");
                }
                return Id;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
