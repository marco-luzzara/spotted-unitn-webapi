using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Test.Mocks.Services;
using SpottedUnitn.Infrastructure.Test.TestingUtility;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.UserAggregate;
using SpottedUnitn.Model.UserAggregate.ValueObjects;
using System;
using System.Collections.Generic;

namespace SpottedUnitn.Model.Test
{
    [TestClass]
    public class UserTest
    {
        public const string VALID_NAME = "myname";
        public const string VALID_LASTNAME = "mylastname";
        public const string VALID_MAIL = "name.lastname@gmail.com";
        public const string VALID_PASSWORD = "123abcABC";
        public static readonly Credentials VALID_CREDENTIALS = Credentials.Create(VALID_MAIL, VALID_PASSWORD);
        public const User.UserRole VALID_USERROLE = User.UserRole.Admin;
        public static readonly byte[] VALID_PROFILEPHOTO = { 0x00, 0x01, 0x02 };

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void Create_NameValidation(string name, bool isValid)
        {
            var validationPassed = true;
            User user = null;
            try
            {
                user = User.Create(name, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int) UserException.UserExceptionCode.InvalidName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(name, user.Name);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_LASTNAME, true)]
        public void Create_LastNameValidation(string lastName, bool isValid)
        {
            var validationPassed = true;
            User user = null;
            try
            {
                user = User.Create(VALID_NAME, lastName, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidLastName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(lastName, user.LastName);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("teststring", false)]
        [DataRow(VALID_MAIL, true)]
        public void CredendialsCreate_MailValidation(string mail, bool isValid)
        {
            var validationPassed = true;
            Credentials creds = null;
            try
            {
                creds = Credentials.Create(mail, VALID_PASSWORD);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidMail)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(mail, creds.Mail);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("pwd<8", false)]
        [DataRow("n0tuppercase", false)]
        [DataRow("N0TLOWERCASE", false)]
        [DataRow("NoTnumbers", false)]
        [DataRow(VALID_PASSWORD, true)]
        public void CredentialsCreate_PasswordValidation(string password, bool isValid)
        {
            var validationPassed = true;
            Credentials creds = null;
            try
            {
                creds = Credentials.Create(VALID_MAIL, password);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidPassword)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.IsNotNull(creds.HashedPwd);
        }

        [TestMethod]
        public void CredentialsEquality_PasswordEqual()
        {
            Credentials creds1 = Credentials.Create(VALID_MAIL, VALID_PASSWORD);
            Credentials creds2 = Credentials.Create(VALID_MAIL, VALID_PASSWORD);

            Assert.AreEqual(creds1, creds2);
        }

        [DataTestMethod]
        [DataRow(VALID_MAIL, VALID_PASSWORD + "a")]
        [DataRow(VALID_MAIL + "a", VALID_PASSWORD)]
        public void CredentialsEquality_WrongCredentials(string mail, string pwd)
        {
            Credentials creds1 = Credentials.Create(VALID_MAIL, VALID_PASSWORD);
            Credentials creds2 = Credentials.Create(mail, pwd);

            Assert.AreNotEqual(creds1, creds2);
        }

        public static IEnumerable<object[]> GetDataRow_ProfilePhotoValidation()
        {
            yield return new object[] { null, false };
            yield return new object[] { new byte[] { }, false };
            yield return new object[] { VALID_PROFILEPHOTO, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataRow_ProfilePhotoValidation), DynamicDataSourceType.Method)]
        public void Create_ProfilePhotoValidation(byte[] profilePhoto, bool isValid)
        {
            var validationPassed = true;
            User user = null;
            try
            {
                user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, profilePhoto, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidProfilePhoto)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                CollectionAssert.AreEqual(profilePhoto, user.ProfilePhoto.ProfilePhoto);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void SetName_ValidationCheck(string name, bool isValid)
        {
            var validationPassed = true;
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            try
            {
                user.SetName(name);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(name, user.Name);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_LASTNAME, true)]
        public void SetLastName_ValidationCheck(string lastName, bool isValid)
        {
            var validationPassed = true;
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            try
            {
                user.SetLastName(lastName);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidLastName)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(lastName, user.LastName);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataRow_ProfilePhotoValidation), DynamicDataSourceType.Method)]
        public void SetProfilePhoto_ValidationCheck(byte[] profilePhoto, bool isValid)
        {
            var validationPassed = true;
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            try
            {
                user.SetProfilePhoto(profilePhoto);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidProfilePhoto)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                CollectionAssert.AreEqual(profilePhoto, user.ProfilePhoto.ProfilePhoto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCredentials_CredentialsNull_Throw()
        {
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);

            user.SetCredentials(null);
        }

        [TestMethod]
        public void SetCredentials_ValidCredentials()
        {
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, VALID_USERROLE);
            var newCredentials = Credentials.Create(VALID_MAIL + "a", VALID_PASSWORD);

            user.SetCredentials(newCredentials);

            Assert.AreEqual(newCredentials, user.Credentials);
        }

        [DataTestMethod]
        [DataRow(User.UserRole.Registered, true, false)]
        [DataRow(User.UserRole.Admin, false, false)]
        [DataRow(User.UserRole.Registered, false, true)]
        public void ChangeRegistrationToConfirmed_VerifyUserAllowed(User.UserRole confirmedUserRole, bool confirmTwice, bool canConfirm)
        {
            var confirmedUser = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, confirmedUserRole);

            var now = DateTimeOffset.Now;
            var dateTimeOffsetService = new DateTimeOffsetServiceMock(now);

            var isConfirmed = true;
            try
            {
                confirmedUser.ChangeRegistrationToConfirmed(dateTimeOffsetService);

                if (confirmTwice)
                    confirmedUser.ChangeRegistrationToConfirmed(dateTimeOffsetService);
            }
            catch (UserException exc) when (exc.Code == (int) UserException.UserExceptionCode.CannotConfirmRegistration)
            {
                isConfirmed = false;
            }

            Assert.AreEqual(canConfirm, isConfirmed);
            if (canConfirm)
                Assert.AreEqual(now, confirmedUser.SubscriptionDate);
        }

        public static IEnumerable<object[]> GetDataRow_IsSubscriptionValid_VerifySubscription()
        {
            var invalidSubscriptionDate = DateTimeOffset.Now + TimeSpan.FromDays(367);
            var dtoServiceMock = new DateTimeOffsetServiceMock(invalidSubscriptionDate);
            var dtoService = new DateTimeOffsetService();

            var admin = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, User.UserRole.Admin);
            yield return new object[] { admin, dtoService, true };

            var registeredExpired = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, User.UserRole.Registered);
            registeredExpired.ChangeRegistrationToConfirmed(dtoService);
            yield return new object[] { registeredExpired, dtoServiceMock, false };

            var registeredUnconfirmed = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, User.UserRole.Registered);
            yield return new object[] { registeredUnconfirmed, dtoService ,false };

            var registeredValid = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, User.UserRole.Registered);
            registeredValid.ChangeRegistrationToConfirmed(dtoService);
            yield return new object[] { registeredValid, dtoService, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataRow_IsSubscriptionValid_VerifySubscription), DynamicDataSourceType.Method)]
        public void IsSubscriptionValid_VerifySubscription(User user, IDateTimeOffsetService dtoService, bool isValid)
        {
            var isSubscriptionValid = user.IsSubscriptionValid(dtoService);

            Assert.AreEqual(isValid, isSubscriptionValid);
        }

        [TestMethod]
        public void GetSubscriptionExpiredDate_UserNotConfirmed_ReturnNull()
        {
            var user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, User.UserRole.Registered);

            var expireDate = user.GetSubscriptionExpiredDate();

            Assert.IsNull(expireDate);
        }

        [DataTestMethod]
        [DataRow(User.UserRole.Registered)]
        [DataRow(User.UserRole.Admin)]
        public void GetSubscriptionExpiredDate_Ok(User.UserRole role)
        {
            var user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_CREDENTIALS, VALID_PROFILEPHOTO, role);
            var dtoService = new DateTimeOffsetServiceMock(DateTimeOffset.Now);
            if (role == User.UserRole.Registered)
                user.ChangeRegistrationToConfirmed(dtoService);

            var expireDate = user.GetSubscriptionExpiredDate();

            switch (role)
            {
                case User.UserRole.Registered:
                    Assert.IsTrue(expireDate == dtoService.Now + TimeSpan.FromDays(365));
                    break;
                case User.UserRole.Admin:
                    Assert.IsNull(expireDate);
                    break;
                default:
                    throw new InvalidOperationException("missing role");
            }
        }
    }
}
