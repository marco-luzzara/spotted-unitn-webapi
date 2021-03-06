﻿using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class ShopException : EntityException
    {
        public ShopException(ShopExceptionCode shopExcCode, string message, params object[] messageParams) 
            : base((int)shopExcCode, message, messageParams)
        {
        }

        public enum ShopExceptionCode
        {
            InvalidName,
            InvalidLinkToSite,
            InvalidDescription,
            InvalidDiscount,
            InvalidCoverPicture,
            InvalidLocationAddress,
            InvalidLocationCity,
            InvalidLocationProvince,
            InvalidLocationPostalCode,
            InvalidLocationLatitude,
            InvalidLocationLongitude,
            InvalidPhoneNumber,
            ShopIdNotFound
        }

        public static ShopException InvalidNameException(string name)
        {
            return new ShopException(ShopExceptionCode.InvalidName, "name cannot be null or empty");
        }

        public static ShopException InvalidLinkToSiteException(string linkToSite)
        {
            return new ShopException(ShopExceptionCode.InvalidLinkToSite, 
                "linkToSite ({0}) does not represent a valid http/s address, use empty string if it does not have a link", linkToSite);
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

        public static ShopException InvalidLocationAddressException(string address)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationAddress, $"address cannot be null or empty");
        }

        public static ShopException InvalidLocationCityException(string city)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationCity, $"city cannot be null or empty");
        }

        public static ShopException InvalidLocationPostalCodeException(string postalCode)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationPostalCode, $"postal code cannot be null or empty");
        }

        public static ShopException InvalidLocationProvinceException(string province)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationProvince, $"province cannot be null or empty");
        }

        public static ShopException InvalidLocationLatitudeException(float latitude)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationLatitude, $"latitude must be in the range [-90, 90]");
        }

        public static ShopException InvalidLocationLongitudineException(float longitude)
        {
            return new ShopException(ShopExceptionCode.InvalidLocationLongitude, $"longitude must be in the range [-180, 180]");
        }

        public static ShopException InvalidPhoneNumberException(string phoneNumber)
        {
            return new ShopException(ShopExceptionCode.InvalidPhoneNumber, 
                "phone number {0} cannot contain letters and cannot exceed 15 digits.", phoneNumber);
        }

        public static ShopException ShopIdNotFoundException(int id)
        {
            return new ShopException(ShopExceptionCode.ShopIdNotFound, "shop (id={0}) has not been found", id);
        }

        public override string GetCodeName()
        {
            return ((ShopExceptionCode)this.Code).ToString();
        }
    }
}
