using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpottedUnitn.Infrastructure.Validation
{
    public class PhoneNumberValidation
    {
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, @"^\+?\d{8,15}$");
        }
    }
}
