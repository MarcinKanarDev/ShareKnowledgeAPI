using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareKnowledgeAPI.Authentication;
using ShareKnowledgeAPI.Database;
using ShareKnowledgeAPI.Entities;
using ShareKnowledgeAPI.Exceptions;
using ShareKnowledgeAPI.Mapper.DTOs;
using ShareKnowledgeAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShareKnowledgeAPI.Implementation
{
    public class UserAccountService : IUserAccountService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public UserAccountService(ApplicationDbContext dbContext, IMapper mapper,
            IPasswordHasher<User> passwordHasher, AuthenticationSettings authentication)
        {
            _authenticationSettings = authentication;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _context = dbContext;
        }

        public string GenerateJwt(LoginDto loginDto)
        {
            var user = _context.Users
                .Include(u => u.Permission)
                .FirstOrDefault(u => u.Email == loginDto.Email);
               

            if (user is null)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Permission.PermissionName}"),
                new Claim(ClaimTypes.DateOfBirth, $"{user.DateOfBirth.ToString()}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer, claims, expires, signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.ToList();

            if (users is null) 
            {
                throw new NotFoundException("Users is empty");
            }

            return users;
        }

        public void RegisterUser(UserRegisterDto userRegisterDto)
        {
            var newUser = new User
            {
                Email = userRegisterDto.Email,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                DateOfBirth = userRegisterDto.DateOfBirth,
                PermissionId = userRegisterDto.PermissionId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, userRegisterDto.Password);

            newUser.HashedPassword = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
