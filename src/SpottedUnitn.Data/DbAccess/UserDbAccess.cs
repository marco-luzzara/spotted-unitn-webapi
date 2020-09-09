using SpottedUnitn.Data.Dto.User;
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
        public UserDbAccess(ModelContext modelContext) : base(modelContext)
        {
        }

        public async Task ConfirmUserRegistration(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllUserUnconfirmedFirst()
        {
            throw new NotImplementedException();
        }

        public async Task<LoggedInUser> Login(Credentials credentials)
        {
            var loggedUserData = this.modelContext.Users
                .Where(u => u.Credentials.Mail == credentials.Mail && u.Credentials.HashedPwd == credentials.HashedPwd)
                .Select(u => new
                {
                    Id = u.Id,
                    Role = u.Role,
                    IsConfirmed = u.SubscriptionDate != null
                }).FirstOrDefault();

            if (loggedUserData == null)
                throw UserException.WrongCredentialsException(credentials.Mail, credentials.HashedPwd);

            if (!loggedUserData.IsConfirmed)
                throw UserException.UserNotConfirmedException(loggedUserData.Id);

            return new LoggedInUser()
            {
                Id = loggedUserData.Id,
                Role = loggedUserData.Role
            };
        }
    }
}
