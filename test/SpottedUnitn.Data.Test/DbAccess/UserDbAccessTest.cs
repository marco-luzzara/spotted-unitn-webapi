using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Data.Test.DbAccess;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.Test;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test
{
    [TestClass]
    public class UserDbAccessTest : EntityDbAccessTest
    {
        protected UserDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new UserDbAccess(ctx);
            return dbAccess;
        }

        protected async Task<LoggedInUser> Login_CredentialsTest(Credentials credentials, bool isConfirmed = true)
        {
            var ctx = this.GetModelContext();
            var dbAccess = GetDbAccessInstance(ctx);
            var user = User.Create(UserTest.VALID_NAME, UserTest.VALID_LASTNAME, UserTest.VALID_CREDENTIALS, UserTest.VALID_PROFILEPHOTO, User.UserRole.Registered);
            if (isConfirmed)
                user.ChangeRegistrationToConfirmed(new DateTimeOffsetService());

            try
            {
                await dbAccess.AddAsync(user);
                await ctx.SaveChangesAsync();

                var loggedUser = await dbAccess.Login(credentials);
                return loggedUser;
            }
            finally
            {
                dbAccess.Delete(user);
                await ctx.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task Login_CredentialsCorrect()
        {
            var loggedUser = await Login_CredentialsTest(UserTest.VALID_CREDENTIALS);
        }

        [TestMethod]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongCredentials)]
        public async Task Login_EmailDoesNotExist_Throw()
        {
            var loggedUser = await Login_CredentialsTest(Credentials.Create(DataGenerator.GenerateMail(), UserTest.VALID_PASSWORD));
        }

        [TestMethod]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongCredentials)]
        public async Task Login_WrongPassword_Throw()
        {
            var loggedUser = await Login_CredentialsTest(Credentials.Create(UserTest.VALID_MAIL, UserTest.VALID_PASSWORD + "1"));
        }

        [TestMethod]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserNotConfirmed)]
        public async Task Login_UserNotConfirmed_Throw()
        {
            var loggedUser = await Login_CredentialsTest(UserTest.VALID_CREDENTIALS, false);
        }
    }
}
