using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Validation;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Net.Mail;

namespace SpottedUnitn.Model.UserAggregate
{
    public class User
    {
        private int id;
        public int Id => this.id;

        private UserRole role;
        public UserRole Role => this.role;

        private string name;
        public string Name => this.name;

        private string lastName;
        public string LastName => this.lastName;

        private Credentials credentials;
        public Credentials Credentials => this.credentials;

        private byte[] profilePhoto;
        public byte[] ProfilePhoto => this.profilePhoto;

        private DateTimeOffset? subscriptionDate;
        public DateTimeOffset? SubscriptionDate => this.subscriptionDate;

        private User()
        {
        }

        public void SetName(string name)
        {
            this.name = ValidateName(name);
        }

        public void SetLastName(string lastName)
        {
            this.lastName = ValidateLastName(lastName);
        }

        public void SetProfilePhoto(byte[] profilePhoto)
        {
            this.profilePhoto = ValidateProfilePhoto(profilePhoto);
        }

        public static User Create(string name, string lastName, string mail, string password, byte[] profilePhoto, UserRole role)
        {
            var user = new User();

            user.SetName(name);
            user.SetLastName(lastName);
            user.credentials = Credentials.Create(mail, password);
            user.SetProfilePhoto(profilePhoto);
            user.subscriptionDate = null;
            user.role = role;

            return user;
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw UserException.InvalidNameException(name);

            return name;
        }

        private static string ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw UserException.InvalidLastNameException(lastName);

            return lastName;
        }

        private static byte[] ValidateProfilePhoto(byte[] profilePhoto)
        {
            if ((profilePhoto?.Length ?? 0) == 0)
                throw UserException.InvalidProfilePhotoException(profilePhoto);

            return profilePhoto;
        }

        public void ConfirmUserRegistration(User confirmingUser, IDateTimeOffsetService dtoService = null)
        {
            if (this.Role == UserRole.Admin && confirmingUser.Role == UserRole.Registered)
                confirmingUser.subscriptionDate = dtoService.Now;
            else
                throw UserException.CannotConfirmRegistrationException(this, confirmingUser);
        }

        public bool IsSubscriptionValid(IDateTimeOffsetService dtoService)
        {
            return this.role == UserRole.Admin || 
                (this.subscriptionDate != null && this.subscriptionDate >= dtoService.Now - TimeSpan.FromDays(365));
        }

        public enum UserRole
        {
            Registered,
            Admin
        }
    }
}
