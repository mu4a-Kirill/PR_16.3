using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR_14.Windows;

namespace CaptchaTests
{
    [TestClass]
    public class CaptchaLogicTests
    {
        [TestMethod]
        public void Captcha_NotRequired_AfterLessThan3FailedAttempts()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");

            Assert.AreEqual("", loginWindow.GetCurrentCaptcha());
            Assert.AreEqual(2, loginWindow.GetFailedAttempts());
        }

        [TestMethod]
        public void Captcha_Required_After3FailedAttempts()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");
            loginWindow.Auth("nonexistent", "wrong3");

            Assert.AreNotEqual("", loginWindow.GetCurrentCaptcha());
            Assert.AreEqual(3, loginWindow.GetFailedAttempts());
        }

        [TestMethod]
        public void Captcha_Code_IsNumericAnd4Digits()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");
            loginWindow.Auth("nonexistent", "wrong3");

            string code = loginWindow.GetCurrentCaptcha();
            Assert.IsTrue(code.Length == 4 && int.TryParse(code, out _));
        }


        [TestMethod]
        public void Captcha_WrongInput_LoginFails()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");
            loginWindow.Auth("nonexistent", "wrong3");
            string wrongCaptcha = "0000";

            bool result = loginWindow.Auth("ivanov", "12345", wrongCaptcha);
            Assert.IsFalse(result);
            Assert.AreEqual("Неверный код с картинки", loginWindow.LastErrorMessage);
        }

        [TestMethod]
        public void Captcha_CorrectInput_After3Failures_LoginSuccess()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");
            loginWindow.Auth("nonexistent", "wrong3");
            string correctCaptcha = loginWindow.GetCurrentCaptcha();

            bool result = loginWindow.Auth("ivanov", "12345", correctCaptcha);
            Assert.IsTrue(result);
            Assert.AreEqual("", loginWindow.LastErrorMessage);
            Assert.AreEqual(0, loginWindow.GetFailedAttempts());
        }

        [TestMethod]
        public void Captcha_AfterSuccessfulLogin_ResetsAndNotRequired()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Auth("nonexistent", "wrong1");
            loginWindow.Auth("nonexistent", "wrong2");
            loginWindow.Auth("nonexistent", "wrong3");
            string captcha = loginWindow.GetCurrentCaptcha();
            loginWindow.Auth("ivanov", "12345", captcha);

            Assert.AreEqual(0, loginWindow.GetFailedAttempts());
            Assert.AreEqual("", loginWindow.GetCurrentCaptcha());
        }

     
    }
}