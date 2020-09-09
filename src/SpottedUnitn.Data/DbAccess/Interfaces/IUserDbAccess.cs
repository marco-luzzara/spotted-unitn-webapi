using SpottedUnitn.Data.DbAccess.Interfaces;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public interface IUserDbAccess : IEntityDbAccess<User>
    {
        Task<List<User>> GetAllUserUnconfirmedFirst();

        Task ConfirmUserRegistration(User user);

        Task<LoggedInUser> Login(Credentials credentials);
    }
}
