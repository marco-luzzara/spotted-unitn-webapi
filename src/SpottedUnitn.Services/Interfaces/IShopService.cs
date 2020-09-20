using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Model.ShopAggregate;
using SpottedUnitn.Services.Dto.Shop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Services.Interfaces
{
    public interface IShopService
    {
        Task<List<ShopBasicInfoDto>> GetShopsAsync();

        Task AddShopAsync(ShopDataDto shop);

        Task<Shop> GetShopInfoAsync(int id);

        Task ChangeShopDataAsync(int shopId, ShopDataDto shopData);

        Task DeleteShopAsync(int id);

        Task<byte[]> GetCoverPictureAsync(int id);
    }
}
