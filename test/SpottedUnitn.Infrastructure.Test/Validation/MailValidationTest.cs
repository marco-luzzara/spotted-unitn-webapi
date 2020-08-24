using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Validation;

namespace SpottedUnitn.Infrastructure.Test
{
    [TestClass]
    public class MailValidationTest
    {
        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow("teststring", false)]
        [DataRow("test1.test2@gmail.com", true)]
        public void IsValid_Test(string mail, bool expectedIsValid)
        {
            var validationResult = MailValidation.IsValid(mail);
            Assert.AreEqual(expectedIsValid, validationResult);
        }
    }
}
