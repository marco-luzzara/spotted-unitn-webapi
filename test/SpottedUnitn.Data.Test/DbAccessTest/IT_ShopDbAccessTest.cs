using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
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

        #region ChangeShopDataAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task ChangeShopDataAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();

            try
            {
                await ctx.Shops.AddAsync(shop);
                await ctx.SaveChangesAsync();

                var newDescription = shop.Description + "_changed";
                shop.SetDescription(newDescription);

                await dbAccess.ChangeShopDataAsync(shop.Id, shop);

                var entityEntry = ctx.ChangeTracker.Entries<Shop>().ToList().Single();
                var dbProps = await entityEntry.GetDatabaseValuesAsync();
                var changedShop = (Shop)dbProps.ToObject();

                Assert.AreEqual(newDescription, changedShop.Description);
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
        public async Task ChangeShopDataAsync_ShopNull_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.ChangeShopDataAsync(0, null);
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(ShopException), (int)ShopException.ShopExceptionCode.ShopIdNotFound)]
        public async Task ChangeShopDataAsync_ShopIdDoesNotExist_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();
            bool isShopDeleted = false;

            try
            {
                await ctx.Shops.AddAsync(shop);
                await ctx.SaveChangesAsync();

                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();
                isShopDeleted = true;

                await dbAccess.ChangeShopDataAsync(shop.Id, shop);
            }
            finally
            {
                if (!isShopDeleted)
                {
                    ctx.Shops.Remove(shop);
                    await ctx.SaveChangesAsync();
                }
            }
        }
        #endregion

        #region DeleteShopAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task DeleteShopAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();
            int shopId = -1;

            try
            {
                await ctx.Shops.AddAsync(shop);
                await ctx.SaveChangesAsync();
                shopId = shop.Id;

                await dbAccess.DeleteShopAsync(shopId);

                Assert.IsNull(await ctx.Shops.FindAsync(shopId));
            }
            finally
            {
                Shop delShop;
                if ((delShop = await ctx.Shops.FindAsync(shopId)) != null)
                {
                    ctx.Shops.Remove(delShop);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(ShopException), (int)ShopException.ShopExceptionCode.ShopIdNotFound)]
        public async Task DeleteShopAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.DeleteShopAsync(int.MaxValue);
        }

        #endregion

        #region GetCoverPictureAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task GetCoverPictureAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();

            try
            {
                await ctx.Shops.AddAsync(shop);
                shop.SetCoverPicture(ShopUtils.VALID_COVERPICTURE);
                await ctx.SaveChangesAsync();

                var coverPicture = await dbAccess.GetCoverPictureAsync(shop.Id);

                CollectionAssert.AreEqual(ShopUtils.VALID_COVERPICTURE, coverPicture);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(ShopException), (int)ShopException.ShopExceptionCode.ShopIdNotFound)]
        public async Task GetCoverPictureAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.GetCoverPictureAsync(int.MaxValue);
        }
        #endregion

        #region GetShopAsync
        [DataTestMethod]
        [DbContextDataSource]
        public async Task GetShopAsync_Ok(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();

            try
            {
                await ctx.Shops.AddAsync(shop);
                shop.SetCoverPicture(ShopUtils.VALID_COVERPICTURE);
                await ctx.SaveChangesAsync();

                var retrievedShop = await dbAccess.GetShopAsync(shop.Id);

                ctx.ChangeTracker.LazyLoadingEnabled = false;
                Assert.AreEqual(shop.Id, retrievedShop.Id);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        [ExpectedEntityException(typeof(ShopException), (int)ShopException.ShopExceptionCode.ShopIdNotFound)]
        public async Task GetShopAsync_IdNotFound_Throw(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);

            await dbAccess.GetShopAsync(int.MaxValue);
        }
        #endregion
    }
}
