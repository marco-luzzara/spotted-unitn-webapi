using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class ShopException : EntityException
    {
        public ShopException(ShopExceptionCode shopExcCode, string message) : base((int)shopExcCode, message)
        {
        }

        public enum ShopExceptionCode
        {
            InvalidName,
            InvalidLinkToSite,
            InvalidDescription,
            InvalidDiscount,
            InvalidCoverPicture
        }

        public static ShopException InvalidNameException(string name)
        {
            return new ShopException(ShopExceptionCode.InvalidName, "name cannot be null or empty");
        }

        public static ShopException InvalidLinkToSiteException(string linkToSite)
        {
            return new ShopException(ShopExceptionCode.InvalidLinkToSite, $"linkToSite ({linkToSite}) does not represent a valid http/s address");
        }

        public static ShopException InvalidDescriptionException(string description)
        {
            return new ShopException(ShopExceptionCode.InvalidDescription, "description cannot be null or empty");
        }

        public static ShopException InvalidDiscountException(string discount)
        {
            return new ShopException(ShopExceptionCode.InvalidDiscount, $"discount cannot be null or empty");
        }

        public static ShopException InvalidCoverPictureException(byte[] coverPicture)
        {
            return new ShopException(ShopExceptionCode.InvalidCoverPicture, "cover picture cannot be an empty array");
        }
    }
}
