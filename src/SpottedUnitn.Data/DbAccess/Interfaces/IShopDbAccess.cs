using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Model.ShopAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public interface IShopDbAccess
    {
        Task<List<ShopBasicInfoDto>> GetAllShopsOrderedByNameAsync();

        Task<Shop> AddShopAsync(Shop shop);

        Task<ShopInfoDto> GetShopAsync(int id);

        Task<Shop> ChangeShopDataAsync(int shopId, Shop shop);

        Task DeleteShopAsync(int id);

        Task<byte[]> GetCoverPictureAsync(int id);
    }
}
