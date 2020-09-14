using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Model.ShopAggregate;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public interface IShopDbAccess
    {
        Task<ShopBasicInfo> GetAllShopsAsync();

        Task<Shop> AddShopAsync(Shop shop);

        Task<Shop> GetShopAsync(int id);

        Task<Shop> ChangeShopDataAsync(int id);

        Task DeleteShopAsync(int id);

        Task<byte[]> GetCoverPictureAsync(int id);
    }
}
