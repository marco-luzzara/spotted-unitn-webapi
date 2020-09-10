using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Test.TestingUtility
{
    public class UserUtils
    {
        public const string VALID_NAME = "myname";
        public const string VALID_LASTNAME = "mylastname";
        public const string VALID_MAIL = "name.lastname@gmail.com";
        public const string VALID_PASSWORD = "123abcABC";
        public static readonly Credentials VALID_CREDENTIALS = Credentials.Create(VALID_MAIL, VALID_PASSWORD);
        public const User.UserRole VALID_USERROLE = User.UserRole.Admin;
        public static readonly byte[] VALID_PROFILEPHOTO = { 0x00, 0x01, 0x02 };
        private static int generator_counter = 0;

        public static User GenerateUser(User.UserRole role = User.UserRole.Registered)
        {
            var user = User.Create(VALID_NAME, 
                VALID_LASTNAME, 
                Credentials.Create(DataGenerator.GenerateMail() + generator_counter, VALID_PASSWORD), 
                VALID_PROFILEPHOTO, 
                role);
            generator_counter++;

            return user;
        }
    }
}
