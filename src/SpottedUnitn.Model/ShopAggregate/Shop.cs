using SpottedUnitn.Infrastructure.Validation;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.ShopAggregate.ValueObjects;
using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SpottedUnitn.Model.ShopAggregate
{
    public class Shop
    {
        private int id;
        public int Id => this.id;

        private string name;
        public string Name => this.name;

        private string linkToSite = "";
        public string LinkToSite => this.linkToSite;

        private string description;
        public string Description => this.description;

        private Location location;
        public virtual Location Location => this.location;

        public virtual ShopCoverPicture CoverPicture { get; private set; }

        private string discount;
        public string Discount => this.discount;

        private string phoneNumber = "";
        public string PhoneNumber => this.phoneNumber;

        protected Shop()
        {
        }

        public static Shop Create(string name, string description, string discount, Location location)
        {
            var shop = new Shop();

            shop.SetName(name);
            shop.SetDescription(description);
            shop.SetDiscount(discount);
            shop.SetLocation(location);
            shop.CoverPicture = new ShopCoverPicture(new byte[] { });
            shop.SetLinkToSite("");
            shop.SetPhoneNumber("");

            return shop;
        }

        public void SetName(string name)
        {
            this.name = ValidateName(name);
        }

        public void SetLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException("location cannot be null");

            this.location = location;
        }

        public void SetDescription(string description)
        {
            this.description = ValidateDescription(description);
        }

        public void SetLinkToSite(string linkToSite)
        {
            this.linkToSite = ValidateLinkToSite(linkToSite);
        }

        public void SetCoverPicture(byte[] coverPicture)
        {
            this.CoverPicture.SetCoverPicture(coverPicture);
        }

        public void SetDiscount(string discount)
        {
            this.discount = ValidateDiscount(discount);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = ValidatePhoneNumber(phoneNumber);
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw ShopException.InvalidNameException(name);

            return name;
        }

        private static string ValidateDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw ShopException.InvalidDescriptionException(description);

            return description;
        }

        private static string ValidateLinkToSite(string linkToSite)
        {
            if (linkToSite == "")
                return linkToSite;

            if (!UrlValidation.IsValid(linkToSite))
                throw ShopException.InvalidLinkToSiteException(linkToSite);

            return linkToSite;
        }

        private static string ValidateDiscount(string discount)
        {
            if (string.IsNullOrEmpty(discount))
                throw ShopException.InvalidDiscountException(discount);

            return discount;
        }

        private static string ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber == "")
                return phoneNumber;

            if (!PhoneNumberValidation.IsValid(phoneNumber))
                throw ShopException.InvalidPhoneNumberException(phoneNumber);

            return phoneNumber;
        }
    }
}
