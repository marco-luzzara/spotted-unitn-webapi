using Microsoft.Data.SqlClient;
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

            try
            {
                await this.modelContext.AddAsync(user);
                await this.modelContext.SaveChangesAsync();
            }
            // duplicate email
            catch (DbUpdateException exc) when ((exc.InnerException as SqlException)?.Number == 2601)
            {
                this.modelContext.Remove(user);
                throw UserException.DuplicateMailException(user.Credentials.Mail);
            }

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

        public async Task<List<UserBasicInfoDto>> GetRegisteredUsersUnconfirmedFirstAsync(int upperLimit)
        {
            if (upperLimit <= 0)
                throw new ArgumentOutOfRangeException("upperLimit must be greater than 0");

            var users = this.modelContext.Users
                .Where(u => u.Role == User.UserRole.Registered)
                .OrderBy(u => u.SubscriptionDate.HasValue)
                .ThenBy(u => u.LastName)
                .Take(upperLimit)
                .Select(u => new UserBasicInfoDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    LastName = u.LastName,
                    Mail = u.Credentials.Mail,
                    IsConfirmed = u.IsSubscriptionValid(this.dtoService),
                    ExpirationDate = u.GetSubscriptionExpiredDate()
                }).AsNoTracking();

            return await users.ToListAsync();
        }

        public async Task<UserBasicInfoDto> GetUserInfoAsync(int id)
        {
            var user = await this.modelContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            return new UserBasicInfoDto()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Mail = user.Credentials.Mail,
                IsConfirmed = user.IsSubscriptionValid(this.dtoService),
                ExpirationDate = user.GetSubscriptionExpiredDate()
            };
        }

        public async Task<byte[]> GetUserProfilePhotoAsync(int id)
        {
            var user = await this.modelContext.Users
                .AsNoTracking()
                .Include(u => u.ProfilePhoto)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw UserException.UserIdNotFoundException(id);

            return user.ProfilePhoto.ProfilePhoto;
        }

        public async Task<LoggedInUserDto> LoginAsync(Credentials credentials)
        {
            var loggedUserData = await this.modelContext.Users
                .Where(u => u.Credentials.Mail == credentials.Mail)
                .AsNoTracking()
                .Select(u => new
                {
                    u.Credentials,
                    u.Id,
                    u.Role,
                    IsConfirmed = u.IsSubscriptionValid(this.dtoService)
                }).FirstOrDefaultAsync();

            if (loggedUserData == null)
                throw UserException.WrongMailException(credentials.Mail);

            if (loggedUserData.Credentials != credentials)
                throw UserException.WrongPasswordException(credentials.Mail);

            if (loggedUserData.Role == User.UserRole.Registered && !loggedUserData.IsConfirmed)
                throw UserException.UserNotConfirmedException(loggedUserData.Id);

            return new LoggedInUserDto()
            {
                Id = loggedUserData.Id,
                Role = loggedUserData.Role
            };
        }
    }
}
