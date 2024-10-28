using AutoMapper;
using BCrypt.Net;
using FurniHub.Models.UserModels;
using FurniHub.Models.UserModels.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniHub.Services.AuthServices
{
    public class AuthService:IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context,IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<string> Register(UserRegisterDTO userDTo)
        {
            try
            {
                if (userDTo == null)
                {
                    throw new ArgumentNullException("user data cannot be null");
                }
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == userDTo.Email);
                if (existingUser != null)
                {
                    return "user already exist";

                }
                string hashPassword=BCrypt.Net.BCrypt.HashPassword(userDTo.Password);
                var user = _mapper.Map<User>(userDTo);
                user.Password = hashPassword;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "registered successfully";
            }
            catch(DbUpdateException ex)
            {
                throw new Exception(ex.InnerException?.Message??ex.Message  );
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        public async Task<string> Login(UserLoginDTO userDTO)
        {
           var user=await _context.Users.FirstOrDefaultAsync(u=>u.Email== userDTO.Email);
            if(user==null || !validatePassword(userDTO.Password,user.Password))
            {
                throw new Exception("invalid email or password");
            }
            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Name,user.UserName),
                new Claim (ClaimTypes.Role, user.Role),
                new Claim (ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool validatePassword(string password,string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
           

    }
}
