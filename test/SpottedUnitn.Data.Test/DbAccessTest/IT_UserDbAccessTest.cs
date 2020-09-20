using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Dto.User;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.Test;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    [TestClass]
    public class IT_UserDbAccessTest : EntityDbAccessTest
    {
        protected IUserDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new UserDbAccess(ctx, this.dtoService);
            return dbAccess;
        }

        #region Login
        protected async Task<LoggedInUserDto> LoginAsync_CredentialsTest(DbContextOptionsBuilder<ModelContext> builder, Credentials credentials, User.UserRole role, bool isConfirmed = true)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = User.Create(UserUtils.VALID_NAME, UserUtils.VALID_LASTNAME, UserUtils.VALID_CREDENTIALS, UserUtils.VALID_PROFILEPHOTO, role);
            if (isConfirmed)
                user.ChangeRegistrationToConfirmed(new DateTimeOffsetService());

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                var loggedUser = await dbAccess.LoginAsync(credentials);
                return loggedUser;
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        public async Task LoginAsync_FromRegistered_CredentialsCorrect(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(builder, UserUtils.VALID_CREDENTIALS, User.UserRole.Registered);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongMail)]
        public async Task LoginAsync_FromRegistered_EmailDoesNotExist_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(
                builder, 
                Credentials.Create(DataGenerator.GenerateMail(), UserUtils.VALID_PASSWORD), 
                User.UserRole.Registered);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongPassword)]
        public async Task LoginAsync_FromRegistered_WrongPassword_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(
                builder, 
                Credentials.Create(UserUtils.VALID_MAIL, UserUtils.VALID_PASSWORD + "1"),
                User.UserRole.Registered);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserNotConfirmed)]
        public async Task LoginAsync_FromRegistered_UserNotConfirmed_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(builder, UserUtils.VALID_CREDENTIALS, User.UserRole.Registered, false);
        }

        [DataTestMethod]
        [DbContextDataSource]
        public async Task LoginAsync_FromAdmin_CredentialsCorrect(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(builder, UserUtils.VALID_CREDENTIALS, User.UserRole.Admin, false);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongMail)]
        public async Task LoginAsync_FromAdmin_EmailDoesNotExist_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(
                builder, 
                Credentials.Create(DataGenerator.GenerateMail(), UserUtils.VALID_PASSWORD), 
                User.UserRole.Admin,
                false);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.WrongPassword)]
        public async Task LoginAsync_FromAdmin_WrongPassword_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var loggedUser = await LoginAsync_CredentialsTest(
                builder, 
                Credentials.Create(UserUtils.VALID_MAIL, UserUtils.VALID_PASSWORD + "1"), 
                User.UserRole.Admin,
                false);
        }
        #endregion

        #region ConfirmUserRegistrationAsync
        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.CannotConfirmRegistration)]
        public async Task ConfirmUserRegistrationAsync_UserAlreadyConfirmed_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser(User.UserRole.Registered);
            user.ChangeRegistrationToConfirmed(new DateTimeOffsetService());

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                await dbAccess.ConfirmUserRegistrationAsync(user.Id);
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        public async Task ConfirmUserRegistrationAsync_UserNotConfirmed_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser(User.UserRole.Registered);

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                Assert.IsFalse(user.IsSubscriptionValid(this.dtoService));
                await dbAccess.ConfirmUserRegistrationAsync(user.Id);

                var entityEntry = ctx.ChangeTracker.Entries<User>().ToList().Single();
                var dbProps = await entityEntry.GetDatabaseValuesAsync();
                var savedUser = (User) dbProps.ToObject();

                Assert.IsTrue(savedUser.IsSubscriptionValid(this.dtoService));
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.CannotConfirmRegistration)]
        public async Task ConfirmUserRegistrationAsync_UserIsAdmin_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser(User.UserRole.Admin);

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                await dbAccess.ConfirmUserRegistrationAsync(user.Id);
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserIdNotFound)]
        public async Task ConfirmUserRegistrationAsync_UserIdDoesNotExist_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.ConfirmUserRegistrationAsync(int.MaxValue);
        }
        #endregion

        #region AddUserAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task AddUserAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser();

            try
            {
                var addedUser = await dbAccess.AddUserAsync(user);

                var entityEntry = ctx.ChangeTracker.Entries<User>().ToList().Single();
                var dbProps = await entityEntry.GetDatabaseValuesAsync();
                var savedUser = (User)dbProps.ToObject();

                Assert.AreEqual(addedUser.Id, savedUser.Id);
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.DuplicateMail)]
        public async Task AddUserAsync_DuplicateMail_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user1 = UserUtils.GenerateUser();
            var user2 = UserUtils.GenerateUser();
            user2.SetCredentials(Credentials.Create(user1.Credentials.Mail, UserUtils.VALID_PASSWORD));
            bool user2Created = false;

            try
            {
                var addedUser1 = await dbAccess.AddUserAsync(user1);
                var addedUser2 = await dbAccess.AddUserAsync(user2);
                user2Created = true;
            }
            finally
            {
                ctx.Users.Remove(user1);
                if (user2Created)
                    ctx.Users.Remove(user2);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AddUserAsync_UserNull_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.AddUserAsync(null);
        }
        #endregion

        #region GetUserInfoAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task GetUserInfoAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser();
            user.ChangeRegistrationToConfirmed(this.dtoService);

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                var userInfo = await dbAccess.GetUserInfoAsync(user.Id);

                Assert.AreEqual(user.Id, userInfo.Id);
                Assert.AreEqual(true, userInfo.IsConfirmed);
                Assert.IsNotNull(userInfo.ExpirationDate);
                Assert.AreEqual(user.Name, userInfo.Name);
                Assert.AreEqual(user.LastName, userInfo.LastName);
                Assert.AreEqual(user.Credentials.Mail, userInfo.Mail);
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserIdNotFound)]
        public async Task GetUserInfoAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var userInfo = await dbAccess.GetUserInfoAsync(int.MaxValue);
        }
        #endregion

        #region DeleteUserAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task DeleteUserAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser();
            int userId = -1;

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();
                userId = user.Id;

                await dbAccess.DeleteUserAsync(userId);

                Assert.IsNull(await ctx.Users.FindAsync(userId));
            }
            finally
            {
                User delUser;
                if ((delUser = await ctx.Users.FindAsync(userId)) != null)
                {
                    ctx.Users.Remove(delUser);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserIdNotFound)]
        public async Task DeleteUserAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.DeleteUserAsync(int.MaxValue);
        }
        #endregion

        #region GetUserProfilePhotoAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task GetUserProfilePhotoAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var user = UserUtils.GenerateUser();

            try
            {
                await ctx.Users.AddAsync(user);
                await ctx.SaveChangesAsync();

                var profilePhoto = await dbAccess.GetUserProfilePhotoAsync(user.Id);

                CollectionAssert.AreEqual(UserUtils.VALID_PROFILEPHOTO, profilePhoto);
            }
            finally
            {
                ctx.Users.Remove(user);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(UserException), (int)UserException.UserExceptionCode.UserIdNotFound)]
        public async Task GetUserProfilePhotoAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.GetUserProfilePhotoAsync(int.MaxValue);
        }
        #endregion
    }
}
