using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpottedUnitn.Model.Test
{
    [TestClass]
    public class UserTest
    {
        private const string VALID_NAME = "myname";
        private const string VALID_LASTNAME = "mylastname";
        private const string VALID_MAIL = "name.lastname@gmail.com";
        private const string VALID_PASSWORD = "123abcABC";
        //private const byte[] VALID_PROFILEPHOTO = { 0x00, 0x01, 0x02 };

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_NAME, true)]
        public void Create_NameValidation(string name, bool isValid)
        {
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_LASTNAME, true)]
        public void Create_LastNameValidation(string lastName, bool isValid)
        {
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(VALID_MAIL, true)]
        public void Create_MailValidation(string mail, bool isValid)
        {
        }
    }
}
