using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using SpottedUnitn.Services.Dto.User;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.WebApi.Configs.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Services
{
    public class UserService : IUserService
    {
        protected readonly IUserDbAccess dbAccess;
        protected readonly JWTOptions jwtOptions;

        public UserService(IUserDbAccess dbAccess, IOptions<JWTOptions> jwtOptions) : this(dbAccess, jwtOptions.Value)
        {
        }

        public UserService(IUserDbAccess dbAccess, JWTOptions jwtOptions)
        {
            this.dbAccess = dbAccess;
            this.jwtOptions = jwtOptions;
        }

        public async Task<User> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmUserRegistrationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserBasicInfoDto> GetUserInfoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetUserProfilePhotoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserBasicInfoDto>> GetUsersAsync(int upperLimit)
        {
            throw new NotImplementedException();
        }

        public async Task<LoggedInUserDto> LoginAsync(UserCredentialsDto credentialsDto)
        {
            var credentials = Credentials.Create(credentialsDto.Mail, credentialsDto.Password);
            var user = await this.dbAccess.LoginAsync(credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }
    }
}
