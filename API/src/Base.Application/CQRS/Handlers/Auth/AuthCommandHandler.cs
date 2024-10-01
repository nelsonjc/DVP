using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers.Auth
{
    public class AuthCommandHandler(
        IConfiguration configuration,
        IMapper mapper, 
        IUserRepository repository, 
        IWorkItemRepository workItemRepository,
        IPasswordService passwordService) : ICommandHandler<AuthCommand, ApiResponse<UserDto>>
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<ApiResponse<UserDto>> Handle(AuthCommand command, CancellationToken cancellation)
        {
            try
            {
                var user = (await _repository.FindAsync(x => x.Active && x.UserName.ToUpper() == command.UserName.ToUpper())).FirstOrDefault() ?? throw new Exception("User or Password incorrect");  

                if (_passwordService.CheckPassword(user.Password, command.Password))
                {
                    if (user.Role.Name.ToUpper() == "ADMINISTRADOR" || user.Role.Name.ToUpper() == "SUPERVISOR")
                    {
                        var tasks = await _workItemRepository.GetAllAsync();
                        user.Tasks = tasks.ToList();
                    }

                    var result = _mapper.Map<UserDto>(user);
                    result.Token = GenerateJwtToken(user);
                    result.RefreshToken = GenerateRefreshJwtToken(user);
                    return ApiResponse<UserDto>.SuccessResponse(result);
                }
                else
                    throw new Exception("User or Password incorrect");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.ErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Method to generate token with data user 
        /// </summary>
        /// <param name="user">Parameter with data user</param>
        /// <returns>Token value</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = _configuration["JwtSettings:SecretKey"];
            var simSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signinCred = new SigningCredentials(simSecKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signinCred);

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                ]),

                Audience = _configuration["JwtSettings:Audience"],
                Issuer = _configuration["JwtSettings:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:ExpiryMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Method to generate refresh token with data user 
        /// </summary>
        /// <param name="user">Parameter with data user</param>
        /// <returns>Token value</returns>
        private string GenerateRefreshJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = _configuration["JwtSettings:SecretKey"];
            var simSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signinCred = new SigningCredentials(simSecKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signinCred);

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                ]),

                Audience = _configuration["JwtSettings:Audience"],
                Issuer = _configuration["JwtSettings:Issuer"],
                Expires = DateTime.UtcNow.AddDays(1).AddHours(-5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
