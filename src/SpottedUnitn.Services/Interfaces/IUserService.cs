using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);

        Task<List<UserBasicInfoDto>> GetUsers(int upperLimit);

        Task ConfirmUserRegistrationAsync(int id);

        Task<LoggedInUserDto> LoginAsync(Credentials credentials);

        Task<UserBasicInfoDto> GetUserInfoAsync(int id);

        Task DeleteUserAsync(int id);

        Task<byte[]> GetUserProfilePhotoAsync(int id);
    }
}
