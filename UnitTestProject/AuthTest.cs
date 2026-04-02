using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR_14.Windows;   

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void AuthTest()
        {
            var loginWindow = new LoginWindow();
 
            Assert.IsTrue(loginWindow.Auth("ivanov", "12345"));

            Assert.IsFalse(loginWindow.Auth("user1", "12345"));

            Assert.IsFalse(loginWindow.Auth("", ""));
            Assert.IsFalse(loginWindow.Auth(" ", " "));
        }
    }
}
