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

        private UserProfilePhoto profilePhoto;
        public UserProfilePhoto ProfilePhoto => this.profilePhoto;

        private DateTimeOffset? subscriptionDate;
        public DateTimeOffset? SubscriptionDate => this.subscriptionDate;

        private const int SUBSCRIPTION_VALIDITY_DAYS = 365;

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

        public static User Create(string name, string lastName, Credentials credentials, byte[] profilePhoto, UserRole role)
        {
            var user = new User();

            user.SetName(name);
            user.SetLastName(lastName);
            user.credentials = credentials;
            user.profilePhoto = new UserProfilePhoto(profilePhoto);
            user.subscriptionDate = null;
            user.role = role;

            return user;
        }

        public void SetProfilePhoto(byte[] profilePhoto)
        {
            this.profilePhoto = new UserProfilePhoto(profilePhoto);
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
                        throw UserException.UserNotConfirmedException(this.id);
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
