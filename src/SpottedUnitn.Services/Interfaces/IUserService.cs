using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using SpottedUnitn.Services.Dto.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserRegisterDto user);

        Task<List<UserBasicInfoDto>> GetUsersAsync();

        Task ConfirmUserRegistrationAsync(int id);

        Task<AuthenticatedUserDto> LoginAsync(UserCredentialsDto credentials);

        Task<UserBasicInfoDto> GetUserInfoAsync(int id);

        Task DeleteUserAsync(int id);

        Task<byte[]> GetUserProfilePhotoAsync(int id);
    }
}
