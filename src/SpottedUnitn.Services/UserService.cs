using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Infrastructure.Conversions;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using SpottedUnitn.Services.Dto.User;
using SpottedUnitn.Services.Interfaces;
using SpottedUnitn.WebApi.Configs.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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

        public async Task AddUserAsync(UserRegisterDto user)
        {
            var profilePhoto = await user.ProfilePhoto.ToByteArrayAsync();
            var credentials = Credentials.Create(user.Mail, user.Password);
            var newUser = User.Create(user.Name, user.LastName, credentials, profilePhoto, User.UserRole.Registered);
            await this.dbAccess.AddUserAsync(newUser);
        }

        public async Task ConfirmUserRegistrationAsync(int id)
        {
            await this.dbAccess.ConfirmUserRegistrationAsync(id);
        }

        public async Task DeleteUserAsync(int id)
        {
            await this.dbAccess.DeleteUserAsync(id);
        }

        public async Task<UserBasicInfoDto> GetUserInfoAsync(int id)
        {
            return await this.dbAccess.GetUserInfoAsync(id);
        }

        public async Task<byte[]> GetUserProfilePhotoAsync(int id)
        {
            return await this.dbAccess.GetUserProfilePhotoAsync(id);
        }

        public async Task<List<UserBasicInfoDto>> GetUsersAsync()
        {
            return await this.dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(100);
        }

        private string RetrieveJwtToken(LoggedInUserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString(), ClaimValueTypes.Integer32),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<AuthenticatedUserDto> LoginAsync(UserCredentialsDto credentialsDto)
        {
            var credentials = Credentials.Create(credentialsDto.Mail, credentialsDto.Password);
            var user = await this.dbAccess.LoginAsync(credentials);
            var token = RetrieveJwtToken(user);

            return new AuthenticatedUserDto()
            {
                Id = user.Id,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}
