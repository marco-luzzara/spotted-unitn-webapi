using SpottedUnitn.Infrastructure.Encryption;
using SpottedUnitn.Infrastructure.Validation;
using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SpottedUnitn.Model.UserAggregate.ValueObjects
{
    public class Credentials
    {
        public string Mail { get; private set; }
        public string HashedPwd { get; private set; }

        protected Credentials()
        {
        }

        public static Credentials Create(string mail, string password)
        {
            var credentials = new Credentials();

            if (!MailValidation.IsValid(mail))
                throw UserException.InvalidMailException(mail);

            if (!credentials.IsPasswordValid(password))
                throw UserException.InvalidPasswordException(password);

            credentials.Mail = mail;
            credentials.HashedPwd = HashingEncryption.EncryptWithBCrypt(password);

            return credentials;
        }

        protected bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$");
        }

        public override bool Equals(object obj)
        {
            Credentials credentialsObj = obj as Credentials;
            if (credentialsObj == null)
                return false;
            else
                return this.Mail == credentialsObj.Mail && this.HashedPwd == credentialsObj.HashedPwd;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mail, HashedPwd);
        }

        public static bool operator ==(Credentials c1, Credentials c2)
        {
            if (c1 is object)
                return c1.Equals(c2);
            else
                return c2 is null;
        }

        public static bool operator !=(Credentials c1, Credentials c2)
        {
            return !(c1 == c2);
        }
    }
}
