using SpottedUnitn.Infrastructure.Validation;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Net.Mail;

namespace SpottedUnitn.Model.UserAggregate
{
    public class User
    {
        private int id;
        public int Id => this.id;

        public string Name { get; set; }
        
        public string LastName { get; set; }

        public Credentials Credentials { get; set; }

        public byte[] ProfilePhoto { get; set; }

        private DateTimeOffset subscriptionDate;
        public DateTimeOffset SubscriptionDate => this.subscriptionDate;

        private User()
        {
        }

        public static User Create(string name, string lastName, string mail, string password, byte[] profilePhoto)
        {
            var user = new User();

            user.Name = name;
            user.LastName = lastName;
            user.Credentials = Credentials.Create(mail, password);
            user.ProfilePhoto = profilePhoto;

            return user;
        }

        public void ChangeToConfirmed()
        {
            throw new NotImplementedException();
        }
    }
}
