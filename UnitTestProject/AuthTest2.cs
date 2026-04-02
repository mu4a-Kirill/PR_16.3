using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR_14.Windows;

namespace UnitTestProject
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void AuthTestSuccess()
        {
            var loginWindow = new LoginWindow();

            Assert.IsTrue(loginWindow.Auth("ivanov", "12345"));
            Assert.IsTrue(loginWindow.Auth("petrov", "qwerty"));
            Assert.IsTrue(loginWindow.Auth("admin", "123456"));
            Assert.IsTrue(loginWindow.Auth("mu4a", "12345"));
        }

        [TestMethod]
        public void AuthTestFail()
        {
            var loginWindow = new LoginWindow();

            // 1. TC_AUTH_02: Неверный пароль
            Assert.IsFalse(loginWindow.Auth("ivanov", "wrongpass"));
            Assert.AreEqual("Неверный логин или пароль", loginWindow.LastErrorMessage);

            // 2. Пустые поля
            Assert.IsFalse(loginWindow.Auth("", ""));
            Assert.AreEqual("Введите логин и пароль!", loginWindow.LastErrorMessage);

            // 3. Несуществующий логин
            Assert.IsFalse(loginWindow.Auth("nonexistent", "12345"));
            Assert.AreEqual("Неверный логин или пароль", loginWindow.LastErrorMessage);

            // 4. TC_AUTH_03: CAPTCHA после 3 неудачных попыток
            loginWindow = new LoginWindow();
            loginWindow.Auth("ivanov", "wrong1");
            loginWindow.Auth("ivanov", "wrong2");
            loginWindow.Auth("ivanov", "wrong3"); 
            string captcha = loginWindow.GetCurrentCaptcha();
            Assert.IsFalse(string.IsNullOrEmpty(captcha), "Капча не сгенерирована");

            Assert.IsFalse(loginWindow.Auth("ivanov", "12345"));
            Assert.AreEqual("Неверный код с картинки", loginWindow.LastErrorMessage);

            Assert.IsTrue(loginWindow.Auth("ivanov", "12345", captcha));
        }
    }
}