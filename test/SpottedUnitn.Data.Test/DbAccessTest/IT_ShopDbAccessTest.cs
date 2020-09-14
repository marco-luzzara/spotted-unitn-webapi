using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    [TestClass]
    public class IT_ShopDbAccessTest : EntityDbAccessTest
    {
        protected IShopDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new ShopDbAccess(ctx, this.dtoService);
            return dbAccess;
        }

        #region AddShopAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task AddShopAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();

            try
            {
                var addedShop = await dbAccess.AddShopAsync(shop);

                var entityEntry = ctx.ChangeTracker.Entries<Shop>().ToList().Single();
                var dbProps = await entityEntry.GetDatabaseValuesAsync();
                var savedShop = (Shop)dbProps.ToObject();

                Assert.AreEqual(addedShop.Id, savedShop.Id);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AddShopAsync_ShopNull_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.AddShopAsync(null);
        }
        #endregion
    }
}
