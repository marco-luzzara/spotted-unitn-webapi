using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Validation;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.Interfaces;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;

namespace SpottedUnitn.Model.UserAggregate
{
    public class User : IEntity<int>
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
        public virtual Credentials Credentials => this.credentials;

        private byte[] profilePhoto;
        public byte[] ProfilePhoto => this.profilePhoto.ToArray();

        private DateTimeOffset? subscriptionDate;
        public DateTimeOffset? SubscriptionDate => this.subscriptionDate;

        private const int SUBSCRIPTION_VALIDITY_DAYS = 365;

        protected User()
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

        public static User Create(string name, string lastName, Credentials credentials, byte[] profilePhoto, UserRole role)
        {
            var user = new User();

            user.SetName(name);
            user.SetLastName(lastName);
            user.SetCredentials(credentials);
            user.SetProfilePhoto(profilePhoto);
            user.subscriptionDate = null;
            user.role = role;

            return user;
        }

        public void SetCredentials(Credentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException("credentials cannot be null");

            this.credentials = credentials;
        }

        public void SetProfilePhoto(byte[] profilePhoto)
        {
            this.profilePhoto = ValidateProfilePhoto(profilePhoto);
        }

        private string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw UserException.InvalidNameException(name);

            return name;
        }

        private string ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw UserException.InvalidLastNameException(lastName);

            return lastName;
        }

        private byte[] ValidateProfilePhoto(byte[] profilePhoto)
        {
            if ((profilePhoto?.Length ?? 0) == 0)
                throw UserException.InvalidProfilePhotoException(profilePhoto);

            return profilePhoto;
        }

        public void ChangeRegistrationToConfirmed(IDateTimeOffsetService dtoService)
        {
            if (this.Role == UserRole.Registered && this.subscriptionDate == null)
                this.subscriptionDate = dtoService.Now;
            else
                throw UserException.CannotConfirmRegistrationException(this);
        }

        public bool IsSubscriptionValid(IDateTimeOffsetService dtoService)
        {
            return this.role == UserRole.Admin || 
                (this.subscriptionDate != null && dtoService.Now <= this.GetSubscriptionExpiredDate());
        }

        public DateTimeOffset? GetSubscriptionExpiredDate()
        {
            switch (this.role)
            {
                case UserRole.Admin:
                    return null;
                case UserRole.Registered:
                    if (subscriptionDate == null)
                        return null;
                    return this.subscriptionDate + TimeSpan.FromDays(SUBSCRIPTION_VALIDITY_DAYS);
                default:
                    throw new InvalidOperationException("missing role");
            }
        }

        public enum UserRole
        {
            Registered,
            Admin
        }
    }
}
