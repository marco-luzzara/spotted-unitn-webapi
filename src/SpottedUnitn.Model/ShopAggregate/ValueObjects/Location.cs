using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpottedUnitn.Model.ShopAggregate.ValueObjects
{
    public class Location
    {
        public string Address { get; private set; }

        public string City { get; private set; }

        public string Province { get; private set; }

        public string PostalCode { get; private set; }

        public float Latitude { get; private set; }

        public float Longitude { get; private set; }

        protected Location()
        {
        }

        public static Location Create(string address, string city, string province, string postalCode, float latitude, float longitude)
        {
            var location = new Location();

            location.Address = ValidateAddress(address);
            location.City = ValidateCity(city);
            location.Province = ValidateProvince(province);
            location.PostalCode = ValidatePostalCode(postalCode);
            location.Latitude = ValidateLatitude(latitude);
            location.Longitude = ValidateLongitude(longitude);

            return location;
        }

        protected static float ValidateLongitude(float longitude)
        {
            if (longitude > 180f || longitude < -180f)
                throw ShopException.InvalidLocationLongitudineException(longitude);

            return longitude;
        }

        protected static float ValidateLatitude(float latitude)
        {
            if (latitude > 90f || latitude < -90f)
                throw ShopException.InvalidLocationLatitudeException(latitude);

            return latitude;
        }

        protected static string ValidatePostalCode(string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode))
                throw ShopException.InvalidLocationPostalCodeException(postalCode);

            return postalCode;
        }

        protected static string ValidateProvince(string province)
        {
            if (string.IsNullOrEmpty(province))
                throw ShopException.InvalidLocationProvinceException(province);

            return province;
        }

        protected static string ValidateCity(string city)
        {
            if (string.IsNullOrEmpty(city))
                throw ShopException.InvalidLocationCityException(city);

            return city;
        }

        protected static string ValidateAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw ShopException.InvalidLocationAddressException(address);

            return address;
        }

        public override bool Equals(object obj)
        {
            Location locationObj = obj as Location;
            if (locationObj == null)
                return false;
            else
                return this.Latitude == locationObj.Latitude && this.Longitude == locationObj.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        public static bool operator ==(Location l1, Location l2)
        {
            if (l1 is object)
                return l1.Equals(l2);
            else
                return l2 is null;
        }

        public static bool operator !=(Location l1, Location l2)
        {
            return !(l1 == l2);
        }
    }
}
