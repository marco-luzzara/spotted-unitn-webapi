using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);

        Task<List<UserBasicInfoDto>> GetUsersAsync(int upperLimit);

        Task ConfirmUserRegistrationAsync(int id);

        Task<LoggedInUserDto> LoginAsync(Credentials credentials);

        Task<UserBasicInfoDto> GetUserInfoAsync(int id);

        Task DeleteUserAsync(int id);

        Task<byte[]> GetUserProfilePhotoAsync(int id);
    }
}
