using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class UserException : EntityException
    {
        public UserException(UserExceptionCode userExcCode, string message) : base ((int) userExcCode, message)
        {
        }

        public enum UserExceptionCode
        {
            INVALID_NAME,
            INVALID_LASTNAME,
            INVALID_MAIL,
            INVALID_PASSWORD,
            INVALID_PROFILE_PHOTO
        }

        public static UserException InvalidNameException(string name)
        {
            return new UserException(UserExceptionCode.INVALID_NAME, "name cannot be null or empty");
        }

        public static UserException InvalidLastNameException(string lastName)
        {
            return new UserException(UserExceptionCode.INVALID_LASTNAME, "lastname cannot be null or empty");
        }

        public static UserException InvalidMailException(string mail)
        {
            return new UserException(UserExceptionCode.INVALID_MAIL, "mail not valid");
        }

        public static UserException InvalidPasswordException(string password)
        {
            return new UserException(UserExceptionCode.INVALID_PASSWORD, $"password ({password}) must be at least 8 characters long and contain letters and digits");
        }

        public static UserException InvalidProfilePhotoException(byte[] profilePhoto)
        {
            return new UserException(UserExceptionCode.INVALID_PROFILE_PHOTO, "profilePhoto cannot be null or empty");
        }
    }
}
