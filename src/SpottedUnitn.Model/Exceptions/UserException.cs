using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Exceptions
{
    public class UserException : EntityException
    {
        public UserException(UserExceptionCode userExcCode, string message, params object[] messageParams) 
            : base ((int) userExcCode, message, messageParams)
        {
        }

        public enum UserExceptionCode
        {
            InvalidName,
            InvalidLastName,
            InvalidMail,
            InvalidPassword,
            InvalidProfilePhoto,
            CannotConfirmRegistration,
            WrongMail,
            WrongPassword,
            UserNotConfirmed,
            UserIdNotFound,
            DuplicateMail
        }

        public static UserException InvalidNameException(string name)
        {
            return new UserException(UserExceptionCode.InvalidName, "name cannot be null or empty");
        }

        public static UserException InvalidLastNameException(string lastName)
        {
            return new UserException(UserExceptionCode.InvalidLastName, "lastname cannot be null or empty");
        }

        public static UserException InvalidMailException(string mail)
        {
            return new UserException(UserExceptionCode.InvalidMail, "mail not valid");
        }

        public static UserException InvalidPasswordException(string password)
        {
            return new UserException(UserExceptionCode.InvalidPassword, $"password must be at least 8 characters long and contain letters and digits");
        }

        public static UserException InvalidProfilePhotoException(byte[] profilePhoto)
        {
            return new UserException(UserExceptionCode.InvalidProfilePhoto, "profilePhoto cannot be null or empty");
        }

        public static UserException CannotConfirmRegistrationException(User confirmed)
        {
            return new UserException(UserExceptionCode.CannotConfirmRegistration, 
                "user (Id={0}, Role={1}) cannot be confirmed because he is admin or has already been confirmed",
                confirmed.Id, confirmed.Role);
        }

        public static UserException WrongMailException(string mail)
        {
            return new UserException(UserExceptionCode.WrongMail, "user with mail {0} does not exist", mail);
        }

        public static UserException WrongPasswordException(string mail)
        {
            return new UserException(UserExceptionCode.WrongPassword, "inserted password is wrong for the user with mail {0}", mail);
        }

        public static UserException UserNotConfirmedException(int id)
        {
            return new UserException(UserExceptionCode.UserNotConfirmed, "user (Id={0}) has not been confirmed", id);
        }

        public static UserException UserIdNotFoundException(int id)
        {
            return new UserException(UserExceptionCode.UserIdNotFound, "user (Id={0}) does not exist", id);
        }

        public static UserException DuplicateMailException(string mail)
        {
            return new UserException(UserExceptionCode.DuplicateMail, "another user has mail {0} associated", mail);
        }
    }
}
