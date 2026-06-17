using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinTrackAPI.Data;
using FinTrackAPI.DTOs;
using FinTrackAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinTrackAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // REGISTRO DE USUÁRIO
        public async Task<User?> RegisterAsync(RegisterDTO dto)
        {
            // Verificar se o e-mail já está cadastrado
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return null;

            // Gerar o hash seguro da senha usando BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Mapear o DTO para a Model de domínio
            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            // Salvar no PostgreSQL
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        // LOGIN DE USUÁRIO
        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            // Buscar o usuário pelo e-mail
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return null; 

            // Verificar a senha usando BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null; 

            // Gerar o token JWT
            return GenerateJwtToken(user);
        }

        // MÉTODO AUXILIAR PARA GERAR O TOKEN JWT
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Token válido por 2 horas
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

}