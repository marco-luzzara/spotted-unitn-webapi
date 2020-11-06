using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Test.Attributes;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.Test.DbAccessTest
{
    [TestClass]
    public class IT_ShopDbAccessTest : EntityDbAccessTest
    {
        protected IShopDbAccess GetDbAccessInstance(ModelContext ctx)
        {
            var dbAccess = new ShopDbAccess(ctx, this.dtoService, this.fileStorageService);
            return dbAccess;
        }

        public IT_ShopDbAccessTest() : base()
        {
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
                var savedShop = await GetObjectFromDbAsync<Shop, int>(ctx, addedShop.Id);

                Assert.IsNotNull(savedShop);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();

                this.SetupFileStorageServiceMock(fssMock =>
                {
                    fssMock.Verify(s => s.StoreFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Once());
                });
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

        [DataTestMethod]
        [DbContextDataSource]
        public async Task AddShopAsync_CannotStorePicture_ShopNotCreated(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();
            this.SetupFileStorageServiceMock(fssMock =>
            {
                fssMock.Setup(s => s.StoreFileAsync(It.IsAny<string>(), shop.CoverPicture, It.IsAny<CancellationToken>())).Throws(new AccessViolationException());
            });

            var shopsCount = await ctx.Shops.CountAsync();
            try
            {
                var addedShop = await dbAccess.AddShopAsync(shop);
                Assert.Fail();
            }
            catch (AccessViolationException exc)
            {
                var shopsNewCount = await ctx.Shops.CountAsync();
                Assert.AreEqual(shopsCount, shopsNewCount);
            }
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
            var newCoverPicture = new byte[] { 0x01 }.ToArray();

            try
            {
                await ctx.Shops.AddAsync(shop);
                await ctx.SaveChangesAsync();

                var newDescription = shop.Description + "_changed";
                shop.SetDescription(newDescription);
                shop.SetCoverPicture(newCoverPicture);

                var changedShop = await dbAccess.ChangeShopDataAsync(shop.Id, shop);
                Shop changedShopFromDb = await GetObjectFromDbAsync<Shop, int>(ctx, shop.Id);

                Assert.AreEqual(newDescription, changedShopFromDb.Description);
                CollectionAssert.AreEqual(newCoverPicture, changedShop.CoverPicture);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();

                this.SetupFileStorageServiceMock(fssMock =>
                {
                    fssMock.Verify(s => s.StoreFileAsync(It.IsAny<string>(), newCoverPicture, It.IsAny<CancellationToken>()), Times.Once());
                });
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        public async Task ChangeShopDataAsync_PictureDoesNotChange_Ok(DbContextOptionsBuilder<ModelContext> builder)
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

                var changedShop = await dbAccess.ChangeShopDataAsync(shop.Id, shop);
            }
            finally
            {
                ctx.Shops.Remove(shop);
                await ctx.SaveChangesAsync();

                this.SetupFileStorageServiceMock(fssMock =>
                {
                    fssMock.Verify(s => s.StoreFileAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()), Times.Never());
                });
            }
        }

        [DataTestMethod]
        [DbContextDataSource]
        public async Task ChangeShopDataAsync_CannotUpdatePicture_Rollback(DbContextOptionsBuilder<ModelContext> builder)
        {
            var ctx = this.GetModelContext(builder);
            var dbAccess = GetDbAccessInstance(ctx);
            var shop = ShopUtils.GenerateShop();
            var newCoverPicture = new byte[] { 0x01 }.ToArray();
            var newDescription = shop.Description + "_changed";
            this.SetupFileStorageServiceMock(fssMock =>
            {
                fssMock.Setup(s => s.StoreFileAsync(It.IsAny<string>(), newCoverPicture, It.IsAny<CancellationToken>())).Throws(new AccessViolationException());
            });

            try
            {
                await ctx.Shops.AddAsync(shop);
                await ctx.SaveChangesAsync();

                shop.SetDescription(newDescription);
                shop.SetCoverPicture(newCoverPicture);

                await dbAccess.ChangeShopDataAsync(shop.Id, shop);
                Assert.Fail();
            }
            catch (AccessViolationException exc)
            {
                var unchangedShop = await GetObjectFromDbAsync<Shop, int>(ctx, shop.Id);
                Assert.AreNotEqual(newDescription, unchangedShop.Description);
                Assert.AreNotEqual(newCoverPicture, unchangedShop.CoverPicture);

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
            this.SetupFileStorageServiceMock(fssMock =>
            {
                fssMock.Verify(s => s.DeleteFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            });

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
            this.SetupFileStorageServiceMock(fssMock =>
            {
                fssMock.Verify(s => s.GetFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            });

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
            this.SetupFileStorageServiceMock(fssMock =>
            {
                fssMock.Verify(s => s.GetFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
            });

            try
            {
                await ctx.Shops.AddAsync(shop);
                shop.SetCoverPicture(ShopUtils.VALID_COVERPICTURE);
                await ctx.SaveChangesAsync();

                var retrievedShop = await dbAccess.GetShopAsync(shop.Id);

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
