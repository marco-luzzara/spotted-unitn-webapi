using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpottedUnitn.Infrastructure.Validation
{
    public class UrlValidation
    {
        public static bool IsValid(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            return Regex.IsMatch(url, @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");
        }
    }
}
