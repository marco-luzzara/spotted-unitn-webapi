using SpottedUnitn.Model.UserAggregate;
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
            InvalidName,
            InvalidLastName,
            InvalidMail,
            InvalidPassword,
            InvalidProfilePhoto,
            CannotConfirmRegistration,
            WrongCredentials,
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
            return new UserException(UserExceptionCode.InvalidPassword, $"password ({password}) must be at least 8 characters long and contain letters and digits");
        }

        public static UserException InvalidProfilePhotoException(byte[] profilePhoto)
        {
            return new UserException(UserExceptionCode.InvalidProfilePhoto, "profilePhoto cannot be null or empty");
        }

        public static UserException CannotConfirmRegistrationException(User confirmed)
        {
            return new UserException(UserExceptionCode.CannotConfirmRegistration, $"user (Id={confirmed.Id}, Role={confirmed.Role}) cannot be confirmed because he is admin or has already been confirmed");
        }

        public static UserException WrongCredentialsException(string mail, string password)
        {
            return new UserException(UserExceptionCode.WrongCredentials, $"credentials (mail={mail}, password={password}) do not reference any user");
        }

        public static UserException UserNotConfirmedException(int id)
        {
            return new UserException(UserExceptionCode.UserNotConfirmed, $"user (Id={id}) has not been confirmed");
        }

        public static UserException UserIdNotFoundException(int id)
        {
            return new UserException(UserExceptionCode.UserIdNotFound, $"user (Id={id}) does not exist");
        }

        public static UserException DuplicateMailException(string mail)
        {
            return new UserException(UserExceptionCode.DuplicateMail, $"another user has mail {mail} associated");
        }
    }
}
