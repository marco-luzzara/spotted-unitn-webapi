using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Services;
using SpottedUnitn.Infrastructure.Test.Mocks.Services;
using SpottedUnitn.Model.Exceptions;
using SpottedUnitn.Model.UserAggregate;
using System;
using System.Collections.Generic;

namespace SpottedUnitn.Model.Test
{
    [TestClass]
    public class UserTest
    {
        private const string VALID_NAME = "myname";
        private const string VALID_LASTNAME = "mylastname";
        private const string VALID_MAIL = "name.lastname@gmail.com";
        private const string VALID_PASSWORD = "123abcABC";
        private const User.UserRole VALID_USERROLE = User.UserRole.Admin;
        private static readonly byte[] VALID_PROFILEPHOTO = { 0x00, 0x01, 0x02 };

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
                user = User.Create(name, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
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
                user = User.Create(VALID_NAME, lastName, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
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
        public void Create_MailValidation(string mail, bool isValid)
        {
            var validationPassed = true;
            User user = null;
            try
            {
                user = User.Create(VALID_NAME, VALID_LASTNAME, mail, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidMail)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(mail, user.Credentials.Mail);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("pwd<8", false)]
        [DataRow("n0tuppercase", false)]
        [DataRow("N0TLOWERCASE", false)]
        [DataRow("NoTnumbers", false)]
        [DataRow(VALID_PASSWORD, true)]
        public void Create_PasswordValidation(string password, bool isValid)
        {
            var validationPassed = true;
            User user = null;
            try
            {
                user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, password, VALID_PROFILEPHOTO, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidPassword)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.IsNotNull(user.Credentials.HashedPwd);
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
                user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, profilePhoto, VALID_USERROLE);
            }
            catch (UserException exc) when (exc.Code == (int)UserException.UserExceptionCode.InvalidProfilePhoto)
            {
                validationPassed = false;
            }

            Assert.AreEqual(isValid, validationPassed);
            if (isValid)
                Assert.AreEqual(profilePhoto, user.ProfilePhoto);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void SetName_ValidationCheck(string name, bool isValid)
        {
            var validationPassed = true;
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
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
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
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
            User user = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, VALID_USERROLE);
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
                Assert.AreEqual(profilePhoto, user.ProfilePhoto);
        }

        [DataTestMethod]
        [DataRow(User.UserRole.Registered, User.UserRole.Registered, false)]
        [DataRow(User.UserRole.Registered, User.UserRole.Admin, false)]
        [DataRow(User.UserRole.Admin, User.UserRole.Registered, true)]
        [DataRow(User.UserRole.Admin, User.UserRole.Admin, false)]
        public void ConfirmUserRegistration_VerifyUserAllowed(User.UserRole confirmeeUserRole, User.UserRole confirmedUserRole, bool canConfirm)
        {
            var confirmeeUser = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, confirmeeUserRole);
            var confirmedUser = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, confirmedUserRole);

            var now = DateTimeOffset.Now;
            var dateTimeOffsetService = new DateTimeOffsetServiceMock(now);

            var isConfirmed = true;
            try
            {
                confirmeeUser.ConfirmUserRegistration(confirmedUser, dateTimeOffsetService);
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

            var admin = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, User.UserRole.Admin);
            yield return new object[] { admin, dtoService, true };

            var registeredInvalid = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, User.UserRole.Registered);
            admin.ConfirmUserRegistration(registeredInvalid, dtoService);
            yield return new object[] { registeredInvalid, dtoServiceMock ,false };

            var registeredUnconfirmed = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, User.UserRole.Registered);
            yield return new object[] { registeredUnconfirmed, dtoService ,false };

            var registeredValid = User.Create(VALID_NAME, VALID_LASTNAME, VALID_MAIL, VALID_PASSWORD, VALID_PROFILEPHOTO, User.UserRole.Registered);
            admin.ConfirmUserRegistration(registeredValid, dtoService);
            yield return new object[] { registeredValid, dtoService, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetDataRow_IsSubscriptionValid_VerifySubscription), DynamicDataSourceType.Method)]
        public void IsSubscriptionValid_VerifySubscription(User user, IDateTimeOffsetService dtoService, bool isValid)
        {
            var isSubscriptionValid = user.IsSubscriptionValid(dtoService);

            Assert.AreEqual(isValid, isSubscriptionValid);
        }
    }
}
