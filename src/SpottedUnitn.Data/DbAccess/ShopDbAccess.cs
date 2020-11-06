using Microsoft.EntityFrameworkCore;
using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Services.FileStorage;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public class ShopDbAccess : EntityDbAccess<Shop>, IShopDbAccess
    {
        public ShopDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService, IFileStorageService fileStorageService) 
            : base(modelContext, dtoService, fileStorageService)
        {
        }

        protected string BuildFileIdFromShopId(int userId) => $"shop_{userId}";

        public async Task<Shop> AddShopAsync(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException("shop cannot be null");

            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            var transaction = await this.modelContext.Database.BeginTransactionAsync();
            
            try
            {
                var newUserTask = this.modelContext.AddAsync(shop, ct).AsTask();
                var saveChangesTask = newUserTask.ContinueWith(t => this.modelContext.SaveChangesAsync(ct)).Unwrap();

                var fileId = BuildFileIdFromShopId(shop.Id);
                var storeFileTask = this.fileStorageService.StoreFileAsync(fileId, shop.CoverPicture, ct);

                await Task.WhenAll(saveChangesTask, storeFileTask);
                await transaction.CommitAsync();
            }
            catch (Exception exc)
            {
                cts.Cancel();
                await transaction.RollbackAsync();
                throw exc;
            }
            finally
            {
                await transaction.DisposeAsync();
            }

            return shop;
        }

        public async Task<Shop> ChangeShopDataAsync(int shopId, Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException("shop cannot be null");

            var updateShop = await this.modelContext.Shops.FindAsync(shopId);

            if (updateShop == null)
                throw ShopException.ShopIdNotFoundException(shopId);

            updateShop.SetName(shop.Name);
            updateShop.SetLinkToSite(shop.LinkToSite);
            updateShop.SetDescription(shop.Description);
            updateShop.SetDiscount(shop.Discount);
            updateShop.SetLocation(shop.Location);
            updateShop.SetPhoneNumber(shop.PhoneNumber);
            updateShop.SetCoverPicture(shop.CoverPicture);

            await using var transaction = await this.modelContext.Database.BeginTransactionAsync();
            await this.modelContext.SaveChangesAsync();

            if (shop.CoverPicture != null)
            {
                var fileId = BuildFileIdFromShopId(shop.Id);
                await this.fileStorageService.StoreFileAsync(fileId, updateShop.CoverPicture);
            }

            await transaction.CommitAsync();

            return updateShop;
        }

        public async Task DeleteShopAsync(int id)
        {
            var shop = await this.modelContext.Shops.FindAsync(id);

            if (shop == null)
                throw ShopException.ShopIdNotFoundException(id);

            this.modelContext.Shops.Remove(shop);
            await this.modelContext.SaveChangesAsync();
        }

        public async Task<List<ShopBasicInfoDto>> GetAllShopsOrderedByNameAsync()
        {
            return await this.modelContext.Shops
                .OrderBy(s => s.Name)
                .Select(s => new ShopBasicInfoDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Discount = s.Discount,
                    Location = s.Location
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<byte[]> GetCoverPictureAsync(int id)
        {
            var shop = await this.modelContext.Shops
                .AsNoTracking()
                .Include(s => s.CoverPicture)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
                throw ShopException.ShopIdNotFoundException(id);

            return shop.CoverPicture;
        }

        public async Task<ShopInfoDto> GetShopAsync(int id)
        {
            var shop = await this.modelContext.Shops
                .Include(s => s.CoverPicture)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
                throw ShopException.ShopIdNotFoundException(id);

            return new ShopInfoDto()
            {
                Id = shop.Id,
                Description = shop.Description,
                Discount = shop.Discount,
                LinkToSite = shop.LinkToSite,
                Location = shop.Location,
                Name = shop.Name,
                PhoneNumber = shop.PhoneNumber
            };
        }
    }
}
