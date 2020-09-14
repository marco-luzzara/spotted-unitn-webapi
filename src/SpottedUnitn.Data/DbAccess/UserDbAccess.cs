using Microsoft.EntityFrameworkCore;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public class UserDbAccess : EntityDbAccess<User>, IUserDbAccess
    {
        public UserDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService) : base(modelContext, dtoService)
        {
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user cannot be null");

            await this.modelContext.AddAsync(user);
            await this.modelContext.SaveChangesAsync();

            return user;
        }

        public async Task ConfirmUserRegistrationAsync(int id)
        {
            var user = await this.modelContext.Users.FindAsync(id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            user.ChangeRegistrationToConfirmed(this.dtoService);
            await this.modelContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await this.modelContext.Users.FindAsync(id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            this.modelContext.Users.Remove(user);
            await this.modelContext.SaveChangesAsync();
        }

        public async Task<List<UserBasicInfo>> GetRegisteredUsersUnconfirmedFirstAsync(int upperLimit)
        {
            if (upperLimit <= 0)
                throw new ArgumentOutOfRangeException("upperLimit must be greater than 0");

            var users = this.modelContext.Users
                .Where(u => u.Role == User.UserRole.Registered)
                .OrderBy(u => u.SubscriptionDate.HasValue)
                .Take(upperLimit)
                .Select(u => new UserBasicInfo()
                {
                    Id = u.Id,
                    Name = u.Name,
                    LastName = u.LastName,
                    Mail = u.Credentials.Mail,
                    IsConfirmed = u.IsSubscriptionValid(this.dtoService)
                }).AsNoTracking();

            return await users.ToListAsync();
        }

        public async Task<UserBasicInfo> GetUserInfoAsync(int id)
        {
            var user = await this.modelContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            return new UserBasicInfo()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Mail = user.Credentials.Mail,
                IsConfirmed = user.IsSubscriptionValid(this.dtoService)
            };
        }

        public async Task<byte[]> GetUserProfilePhotoAsync(int id)
        {
            var user = await this.modelContext.Users.FindAsync(id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            return user.ProfilePhoto.ProfilePhoto;
        }

        public async Task<LoggedInUser> LoginAsync(Credentials credentials)
        {
            var loggedUserData = await this.modelContext.Users
                .Where(u => u.Credentials.Mail == credentials.Mail && u.Credentials.HashedPwd == credentials.HashedPwd)
                .Select(u => new
                {
                    u.Id,
                    u.Role,
                    IsConfirmed = u.SubscriptionDate != null
                }).AsNoTracking().FirstOrDefaultAsync();

            if (loggedUserData == null)
                throw UserException.WrongCredentialsException(credentials.Mail, credentials.HashedPwd);

            if (loggedUserData.Role == User.UserRole.Registered && !loggedUserData.IsConfirmed)
                throw UserException.UserNotConfirmedException(loggedUserData.Id);

            return new LoggedInUser()
            {
                Id = loggedUserData.Id,
                Role = loggedUserData.Role
            };
        }
    }
}
