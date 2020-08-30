using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SpottedUnitn.Model.Test
{
    [TestClass]
    class ShopTest
    {
        private const string VALID_NAME = "myshop";
        private const string VALID_DESCRIPTION = "description";
        private const string VALID_LINKTOSITE = "http://myshop.com";
        private const string VALID_DISCOUNT = "10%";
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
                shop = Shop.Create(name, VALID_DESCRIPTION, VALID_DISCOUNT);
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
                shop = Shop.Create(VALID_NAME, description, VALID_DISCOUNT);
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
                shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, discount);
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
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void SetName_ValidationCheck(string name, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT);
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
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT);
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

        public static IEnumerable<object[]> GetDataRow_CoverPictureValidation()
        {
            yield return new object[] { null, false };
            yield return new object[] { new byte[] { }, true };
            yield return new object[] { VALID_COVERPICTURE, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataRow_CoverPictureValidation), DynamicDataSourceType.Method)]
        public void SetCoverPicture_ValidationCheck(byte[] coverPicture, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT);
            try
            {
                shop.SetCoverPicture(coverPicture);
            }
            catch (ShopException exc) when (exc.Code == (int)ShopException.ShopExceptionCode.InvalidCoverPicture)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(coverPicture, shop.CoverPicture);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", true)]
        [DataRow("http:/testing.com", false)]
        [DataRow(VALID_LINKTOSITE, true)]
        public void SetLinkToSite_ValidationCheck(string linkToSite, bool isValid)
        {
            var validationPassed = true;
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT);
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
            Shop shop = Shop.Create(VALID_NAME, VALID_DESCRIPTION, VALID_DISCOUNT);
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
    }
}
