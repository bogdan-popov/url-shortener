using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UrlShortener.Configuration;
using UrlShortener.Data;
using UrlShortener.DTOs;
using UrlShortener.Models;
using UrlShortener.Repositories;
using UrlShortener.Services.PasswordHasher;

namespace UrlShortener.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettings _settings;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IOptions<JwtSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _settings = settings.Value;
        }

        public async Task<int> Register(UserRegisterDto request)
        {
            _passwordHasher.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return user.Id;
        }

        public async Task<string> Login(UserRegisterDto request)
        {
            var user = await _unitOfWork.Users.FindByUsernameAsync(request.Username);

            if (user == null || !_passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) 
            {
                throw new Exception("Неверный логин или пароль.");
            }

            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
