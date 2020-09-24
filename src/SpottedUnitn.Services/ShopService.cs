using SpottedUnitn.Data.DbAccess;
using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Infrastructure.Conversions;
using SpottedUnitn.Model.ShopAggregate;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using SpottedUnitn.Services.Dto.Shop;
using SpottedUnitn.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpottedUnitn.Services
{
    public class ShopService : IShopService
    {
        protected readonly IShopDbAccess dbAccess;

        public ShopService(IShopDbAccess dbAccess)
        {
            this.dbAccess = dbAccess;
        }

        public async Task AddShopAsync(ShopDataDto shopData)
        {
            var shop = await shopData.ToShop();
            await this.dbAccess.AddShopAsync(shop);
        }

        public async Task ChangeShopDataAsync(int shopId, ShopDataDto shopData)
        {
            var shop = await shopData.ToShop();
            await this.dbAccess.ChangeShopDataAsync(shopId, shop);
        }

        public async Task DeleteShopAsync(int id)
        {
            await this.dbAccess.DeleteShopAsync(id);
        }

        public async Task<List<ShopBasicInfoDto>> GetShopsAsync()
        {
            return await this.dbAccess.GetAllShopsOrderedByNameAsync();
        }

        public async Task<byte[]> GetCoverPictureAsync(int id)
        {
            return await this.dbAccess.GetCoverPictureAsync(id);
        }

        public async Task<Shop> GetShopInfoAsync(int id)
        {
            return await this.dbAccess.GetShopAsync(id);
        }
    }
}
