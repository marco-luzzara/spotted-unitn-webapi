using Microsoft.EntityFrameworkCore;
using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public class ShopDbAccess : EntityDbAccess<Shop>, IShopDbAccess
    {
        public ShopDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService) : base(modelContext, dtoService)
        {
        }

        public async Task<Shop> AddShopAsync(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException("shop cannot be null");

            await this.modelContext.AddAsync(shop);
            await this.modelContext.SaveChangesAsync();

            return shop;
        }

        public async Task<Shop> ChangeShopDataAsync(Shop shop)
        {
            if (shop == null)
                throw new ArgumentNullException("shop cannot be null");

            var updateShop = await this.modelContext.Shops.FindAsync(shop.Id);

            if (updateShop == null)
                throw ShopException.ShopIdNotFoundException(shop.Id);

            updateShop.SetName(shop.Name);
            updateShop.SetLinkToSite(shop.LinkToSite);
            updateShop.SetDescription(shop.Description);
            updateShop.SetDiscount(shop.Discount);
            updateShop.SetLocation(shop.Location);
            updateShop.SetPhoneNumber(shop.PhoneNumber);
            updateShop.SetCoverPicture(shop.CoverPicture.CoverPicture);

            await this.modelContext.SaveChangesAsync();

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

        public async Task<List<ShopBasicInfo>> GetAllShopsOrderedByNameAsync()
        {
            return await this.modelContext.Shops
                .OrderBy(s => s.Name)
                .Select(s => new ShopBasicInfo()
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

            return shop.CoverPicture.CoverPicture;
        }

        public async Task<Shop> GetShopAsync(int id)
        {
            var shop = await this.modelContext.Shops
                .Include(s => s.CoverPicture)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
                throw ShopException.ShopIdNotFoundException(id);

            return shop;
        }
    }
}
