using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    [TestClass]
    public class UT_UserDbAccessTest : EntityDbAccessTest
    {
        protected UserDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new UserDbAccess(ctx, this.dtoService);
            return dbAccess;
        }

        #region GetAllUserUnconfirmedFirst
        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_EmptyList(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var users = await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(10);

            Assert.AreEqual(0, users.Count);
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_AdminExcluded(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var users = new List<User>()
            {
                UserUtils.GenerateUser(User.UserRole.Admin),
                UserUtils.GenerateUser(User.UserRole.Admin),
                UserUtils.GenerateUser()
            };

            try
            {
                await ctx.Users.AddRangeAsync(users);
                await ctx.SaveChangesAsync();

                var retUsers = await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(3);

                Assert.AreEqual(1, retUsers.Count);
                Assert.AreEqual(users[2].Id, retUsers[0].Id);
            }
            finally
            {
                ctx.Users.RemoveRange(users);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_SomeUnconfirmed(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var users = new List<User>()
            {
                UserUtils.GenerateUser(),
                UserUtils.GenerateUser(),
                UserUtils.GenerateUser()
            };
            var dtoService = new DateTimeOffsetService();
            users[0].ChangeRegistrationToConfirmed(dtoService);
            users[2].ChangeRegistrationToConfirmed(dtoService);

            try
            {
                await ctx.Users.AddRangeAsync(users);
                await ctx.SaveChangesAsync();

                var retUsers = await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(3);

                Assert.AreEqual(3, retUsers.Count);
                Assert.AreEqual(users[1].Id, retUsers[0].Id);
            }
            finally
            {
                ctx.Users.RemoveRange(users);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_OrderedByLastName(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var user1 = UserUtils.GenerateUser();
            user1.SetLastName("bbb");
            var user2 = UserUtils.GenerateUser();
            user2.SetLastName("ccc");
            var user3 = UserUtils.GenerateUser();
            user3.SetLastName("aaa");

            var users = new List<User>() { user1, user2, user3 };

            try
            {
                await ctx.Users.AddRangeAsync(users);
                await ctx.SaveChangesAsync();

                var retUsers = await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(3);

                Assert.AreEqual(3, retUsers.Count);
                CollectionAssert.AreEqual(new string[] { "aaa", "bbb", "ccc" }, retUsers.Select(u => u.LastName).ToArray());
            }
            finally
            {
                ctx.Users.RemoveRange(users);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_UpperLimitOutOfRange_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(0);
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetRegisteredUsersUnconfirmedFirstAsync_UpperLimitLessThanUsers(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var users = new List<User>()
            {
                UserUtils.GenerateUser(),
                UserUtils.GenerateUser(),
                UserUtils.GenerateUser()
            };
            var dtoService = new DateTimeOffsetService();
            users[0].ChangeRegistrationToConfirmed(dtoService);
            users[2].ChangeRegistrationToConfirmed(dtoService);

            try
            {
                await ctx.Users.AddRangeAsync(users);
                await ctx.SaveChangesAsync();

                var retUsers = await dbAccess.GetRegisteredUsersUnconfirmedFirstAsync(1);

                Assert.AreEqual(1, retUsers.Count);
                Assert.AreEqual(users[1].Id, retUsers[0].Id);
            }
            finally
            {
                ctx.Users.RemoveRange(users);
                await ctx.SaveChangesAsync();
            }
        }
        #endregion
    }
}
