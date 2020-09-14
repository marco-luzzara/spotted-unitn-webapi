using SpottedUnitn.Data.Dto.Shop;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpottedUnitn.Data.DbAccess
{
    public class ShopDbAccess : EntityDbAccess<Shop>, IShopDbAccess
    {
        public ShopDbAccess(ModelContext modelContext, IDateTimeOffsetService dtoService) : base(modelContext, dtoService)
        {
        }

        public Task<Shop> AddShopAsync(Shop shop)
        {
            throw new NotImplementedException();
        }

        public Task<Shop> ChangeShopDataAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteShopAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ShopBasicInfo> GetAllShopsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetCoverPictureAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Shop> GetShopAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
