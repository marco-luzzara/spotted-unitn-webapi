using SpottedUnitn.Model.UserAggregate;
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
            InvalidCoverPicture,
            CannotCreate
        }

        public static ShopException InvalidNameException(string name)
        {
            return new ShopException(ShopExceptionCode.InvalidName, "name cannot be null or empty");
        }

        public static ShopException InvalidLinkToSiteException(string linkToSite)
        {
            return new ShopException(ShopExceptionCode.InvalidLinkToSite, $"linkToSite ({linkToSite}) does not represent a valid http/s address, use empty string if it does not have a link");
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
            return new ShopException(ShopExceptionCode.InvalidCoverPicture, "cover picture cannot be null, use an empty array to specify no picture");
        }

        public static ShopException CannotCreateException(User user)
        {
            return new ShopException(ShopExceptionCode.CannotCreate, $"user (Id={user.Id}, Role={user.Role}) cannot add a new shop. Only admin can");
        }
    }
}
