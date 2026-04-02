using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR_14.Windows;

namespace RegistrationTestsProject
{
    [TestClass]
    public class RegistrationTests
    {
        // Позитивный тест (TC_REG_01)
        [TestMethod]
        public void Register_Success_Test()
        {
            var regWindow = new RegistraciyaWindow();
            string uniqueLogin = $"testuser_{System.Guid.NewGuid():N}";
            string password = "Hgjaf23";
            string imya = "Анна";
            string familiya = "Новикова";

            bool result = regWindow.Register(uniqueLogin, password, imya, familiya, out string errorMessage);

            Assert.IsTrue(result, "Регистрация должна быть успешной");
            Assert.AreEqual("", errorMessage);
        }

        // Негативный тест: пустой логин (TC_REG_02)
        [TestMethod]
        public void Register_EmptyLogin_Test()
        {
            var regWindow = new RegistraciyaWindow();
            bool result = regWindow.Register("", "pass", "Имя", "Фамилия", out string error);
            Assert.IsFalse(result);
            Assert.AreEqual("Логин обязателен для заполнения", error);
        }

        // Негативный тест: пустой пароль (TC_REG_02)
        [TestMethod]
        public void Register_EmptyPassword_Test()
        {
            var regWindow = new RegistraciyaWindow();
            bool result = regWindow.Register("login", "", "Имя", "Фамилия", out string error);
            Assert.IsFalse(result);
            Assert.AreEqual("Пароль обязателен для заполнения", error);
        }

        // Негативный тест: пустое имя (TC_REG_02)
        [TestMethod]
        public void Register_EmptyFirstName_Test()
        {
            var regWindow = new RegistraciyaWindow();
            bool result = regWindow.Register("login", "pass", "", "Фамилия", out string error);
            Assert.IsFalse(result);
            Assert.AreEqual("Имя обязательно для заполнения", error);
        }

        // Негативный тест: пустая фамилия (TC_REG_02)
        [TestMethod]
        public void Register_EmptyLastName_Test()
        {
            var regWindow = new RegistraciyaWindow();
            bool result = regWindow.Register("login", "pass", "Имя", "", out string error);
            Assert.IsFalse(result);
            Assert.AreEqual("Фамилия обязательна для заполнения", error);
        }

        // Негативный тест: дублирование логина (TC_REG_03)
        [TestMethod]
        public void Register_DuplicateLogin_Test()
        {
            var regWindow = new RegistraciyaWindow();
            bool result = regWindow.Register("ivanov", "pass", "Имя", "Фамилия", out string error);
            Assert.IsFalse(result);
            Assert.AreEqual("Пользователь с таким логином уже существует", error);
        }
    }
}
