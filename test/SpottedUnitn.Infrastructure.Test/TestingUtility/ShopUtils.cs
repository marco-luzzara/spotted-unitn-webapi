using SpottedUnitn.Model.ShopAggregate;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Test.TestingUtility
{
    public class ShopUtils
    {
        public const string VALID_NAME = "myshop";
        public const string VALID_DESCRIPTION = "description";
        public const string VALID_LINKTOSITE = "http://myshop.com";
        public const string VALID_DISCOUNT = "10%";
        public const string VALID_ADDRESS = "via testing, 10";
        public const string VALID_CITY = "Trento";
        public const string VALID_PROVINCE = "TN";
        public const string VALID_POSTALCODE = "40000";
        public const float VALID_LATITUDE = 0;
        public const float VALID_LONGITUDE = 0;
        private const string VALID_PHONE_NUMBER = "3334445556";
        public static readonly Location VALID_LOCATION = Location.Create(VALID_ADDRESS, VALID_CITY, VALID_PROVINCE, VALID_POSTALCODE, VALID_LATITUDE, VALID_LONGITUDE);
        public static readonly byte[] VALID_COVERPICTURE = { 0x00, 0x01, 0x02 };

        public static Shop GenerateShop(string name = null)
        {
            var shop = Shop.Create(name ?? VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);

            return shop;
        }
    }
}
