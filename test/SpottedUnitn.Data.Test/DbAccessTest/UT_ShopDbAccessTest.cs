using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    [TestClass]
    public class UT_ShopDbAccessTest : EntityDbAccessTest
    {
        protected IShopDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new ShopDbAccess(ctx, this.dtoService);
            return dbAccess;
        }

        #region GetAllShopsOrderedByNameAsync
        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetAllShopsOrderedByNameAsync_EmptyList(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var shops = await dbAccess.GetAllShopsOrderedByNameAsync();

            Assert.AreEqual(0, shops.Count);
        }

        [DataTestMethod]
        [InMemoryContextDataSource]
        public async Task GetAllShopsOrderedByNameAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            var shops = new List<Shop>()
            {
                ShopUtils.GenerateShop("b_shop"),
                ShopUtils.GenerateShop("c_shop"),
                ShopUtils.GenerateShop("a_shop")
            };

            try
            {
                await ctx.Shops.AddRangeAsync(shops);
                await ctx.SaveChangesAsync();

                var retShops = await dbAccess.GetAllShopsOrderedByNameAsync();

                Assert.AreEqual(shops[2].Name, retShops[0].Name);
                Assert.AreEqual(shops[0].Name, retShops[1].Name);
                Assert.AreEqual(shops[1].Name, retShops[2].Name);
            }
            finally
            {
                ctx.Shops.RemoveRange(shops);
                await ctx.SaveChangesAsync();
            }
        }
        #endregion
    }
}
