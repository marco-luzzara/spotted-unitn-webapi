using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Infrastructure.Validation;

namespace SpottedUnitn.Infrastructure.Test
{
    [TestClass]
    public class UrlValidationTest
    {
        [DataTestMethod]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataRow("www.google.com", false)]
        [DataRow("http://test", false)]
        [DataRow("http://test.com", true)]
        [DataRow("https://test.test2.com", true)]
        public void IsValid_Test(string url, bool expectedIsValid)
        {
            var validationResult = UrlValidation.IsValid(url);
            Assert.AreEqual(expectedIsValid, validationResult);
        }
    }
}
