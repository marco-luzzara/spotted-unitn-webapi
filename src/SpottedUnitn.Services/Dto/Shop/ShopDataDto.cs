using Microsoft.AspNetCore.Http;
using SpottedUnitn.Infrastructure.Conversions;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopModel = SpottedUnitn.Model.ShopAggregate.Shop;

namespace SpottedUnitn.Services.Dto.Shop
{
    public class ShopDataDto
    {
        public string Name { get; set;  }

        public string LinkToSite { get; set; }
        
        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public IFormFile CoverPicture { get; set; }
        
        public string Discount { get; set; }
        
        public string PhoneNumber { get; set; }

        public async Task<ShopModel> ToShop()
        {
            var location = Location.Create(this.Address, this.City, this.Province, this.PostalCode, this.Latitude, this.Longitude);
            var shop = ShopModel.Create(this.Name, this.Description, this.Discount, location);
            shop.SetCoverPicture(await this.CoverPicture.ToByteArrayAsync());
            shop.SetLinkToSite(this.LinkToSite);
            shop.SetPhoneNumber(this.PhoneNumber);

            return shop;
        }
    }
}
