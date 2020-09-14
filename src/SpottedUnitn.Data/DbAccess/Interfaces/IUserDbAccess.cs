using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public interface IUserDbAccess
    {
        Task<User> AddUserAsync(User user);

        Task<List<UserBasicInfo>> GetRegisteredUsersUnconfirmedFirstAsync(int upperLimit);

        Task ConfirmUserRegistrationAsync(int id);

        Task<LoggedInUser> LoginAsync(Credentials credentials);

        Task<UserBasicInfo> GetUserInfoAsync(int id);

        Task DeleteUserAsync(int id);

        Task<byte[]> GetUserProfilePhotoAsync(int id);
    }
}
