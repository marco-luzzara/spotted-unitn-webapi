using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SpottedUnitn.Model.Test
{
    [TestClass]
    public class ShopTest
    {
        private const string VALID_NAME = "myshop";
        private const string VALID_DESCRIPTION = "description";
        private const string VALID_LINKTOSITE = "http://myshop.com";
        private const string VALID_DISCOUNT = "10%";
        private const string VALID_ADDRESS = "via testing, 10";
        private const string VALID_CITY = "Trento";
        private const string VALID_PROVINCE = "TN";
        private const string VALID_POSTALCODE = "40000";
        private const float VALID_LATITUDE = 0;
        private const float VALID_LONGITUDE = 0;
        private const string VALID_PHONE_NUMBER = "3334445556";
        private static readonly Location VALID_LOCATION = Location.Create(VALID_ADDRESS, VALID_CITY, VALID_PROVINCE, VALID_POSTALCODE, VALID_LATITUDE, VALID_LONGITUDE);
        private static readonly byte[] VALID_COVERPICTURE = { 0x00, 0x01, 0x02 };

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void Create_NameValidation(string name, bool isValid)
        {
            var validationPassed = true;
            Shop shop = null;
            try
            {
                shop = Shop.Create(name, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(name, shop.Name);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_DESCRIPTION, true)]
        public void Create_DescriptionValidation(string description, bool isValid)
        {
            var validationPassed = true;
            Shop shop = null;
            try
            {
                shop = Shop.Create(VALID_NAME, description, VALID_DISCOUNT, VALID_LOCATION);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidDescription)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(description, shop.Description);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_DISCOUNT, true)]
        public void Create_DiscountValidation(string discount, bool isValid)
        {
            var validationPassed = true;
            Shop shop = null;
            try
            {
                shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, discount, VALID_LOCATION);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidDiscount)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(discount, shop.Discount);
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow(VALID_ADDRESS, true)]
        public void LocationCreate_AddressValidation(string address, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(address, VALID_CITY, VALID_PROVINCE, VALID_POSTALCODE, VALID_LATITUDE, VALID_LONGITUDE);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationAddress)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(address, location.Address);
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow(VALID_CITY, true)]
        public void LocationCreate_CityValidation(string city, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(VALID_ADDRESS, city, VALID_PROVINCE, VALID_POSTALCODE, VALID_LATITUDE, VALID_LONGITUDE);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationCity)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(city, location.City);
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow(VALID_PROVINCE, true)]
        public void LocationCreate_ProvinceValidation(string province, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(VALID_ADDRESS, VALID_CITY, province, VALID_POSTALCODE, VALID_LATITUDE, VALID_LONGITUDE);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationProvince)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(province, location.Province);
        }

        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow(VALID_POSTALCODE, true)]
        public void LocationCreate_PostalCodeValidation(string postalCode, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(VALID_ADDRESS, VALID_CITY, VALID_PROVINCE, postalCode, VALID_LATITUDE, VALID_LONGITUDE);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationPostalCode)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(postalCode, location.PostalCode);
        }

        [DataTestMethod]
        [DataRow(91f, false)]
        [DataRow(-91f, false)]
        [DataRow(VALID_LATITUDE, true)]
        public void LocationCreate_LatitudeValidation(float latitude, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(VALID_ADDRESS, VALID_CITY, VALID_PROVINCE, VALID_POSTALCODE, latitude, VALID_LONGITUDE);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationLatitude)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(latitude, location.Latitude);
        }

        [DataTestMethod]
        [DataRow(181f, false)]
        [DataRow(-181f, false)]
        [DataRow(VALID_LONGITUDE, true)]
        public void LocationCreate_LongitudeValidation(float longitude, bool isValid)
        {
            var validationPassed = true;
            Location location = null;
            try
            {
                location = Location.Create(VALID_ADDRESS, VALID_CITY, VALID_PROVINCE, VALID_POSTALCODE, VALID_LATITUDE, longitude);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLocationLongitude)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(longitude, location.Longitude);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void SetName_ValidationCheck(string name, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            try
            {
                shop.SetName(name);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(name, shop.Name);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_DESCRIPTION, true)]
        public void SetDescription_ValidationCheck(string description, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            try
            {
                shop.SetDescription(description);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidDescription)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(description, shop.Description);
        }

        [TestMethod]
        [ExpectedEntityException(typeof(ShopException), (int)ShopException.ShopExceptionCode.InvalidCoverPicture)]
        public void SetCoverPicture_CoverPictureEmpty()
        {
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);

            shop.SetCoverPicture(new byte[] { });
        }

        [TestMethod]
        public void SetCoverPicture_CoverPictureNull()
        {
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);

            shop.SetCoverPicture(VALID_COVERPICTURE);
            shop.SetCoverPicture(null);

            CollectionAssert.AreEqual(VALID_COVERPICTURE, shop.CoverPicture);
        }

        [TestMethod]
        public void SetCoverPicture_CoverPictureValid()
        {
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            var newCoverPicture = VALID_COVERPICTURE.ToList().Append((byte)0).ToArray();

            shop.SetCoverPicture(newCoverPicture);

            CollectionAssert.AreEqual(newCoverPicture, shop.CoverPicture);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", true)]
        [DataRow("http:/testing.com", false)]
        [DataRow(VALID_LINKTOSITE, true)]
        public void SetLinkToSite_ValidationCheck(string linkToSite, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            try
            {
                shop.SetLinkToSite(linkToSite);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidLinkToSite)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(linkToSite, shop.LinkToSite);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_DISCOUNT, true)]
        public void SetDiscount_ValidationCheck(string discount, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            try
            {
                shop.SetDiscount(discount);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidDiscount)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(discount, shop.Discount);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("123aa345", false)]
        [DataRow("", true)]
        [DataRow(VALID_PHONE_NUMBER, true)]
        public void SetPhoneNumber_ValidationCheck(string phoneNumber, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);
            try
            {
                shop.SetPhoneNumber(phoneNumber);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidPhoneNumber)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(phoneNumber, shop.PhoneNumber);
        }

        public static IEnumerable<object[]> GetDataRow_LocationValidation()
        {
            yield return new object[] { null, false };
            yield return new object[] { VALID_LOCATION, true };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetLocation_LocationNull_Throw()
        {
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);

            shop.SetLocation(null);
        }

        [TestMethod]
        public void SetLocation_LocatioValid()
        {
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT, VALID_LOCATION);

            shop.SetLocation(VALID_LOCATION);

            Assert.AreEqual(VALID_LOCATION, shop.Location);
        }
    }
}
