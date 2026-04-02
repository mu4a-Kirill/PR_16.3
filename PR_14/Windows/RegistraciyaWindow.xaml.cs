using System.Windows;

namespace PR_14.Windows
{
    public partial class RegistraciyaWindow : Window
    {
        private DatabaseHelper dbHelper;

        public RegistraciyaWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Метод регистрации для модульного тестирования
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="imya">Имя</param>
        /// <param name="familiya">Фамилия</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <returns>true - успех, false - ошибка</returns>
        public bool Register(string login, string password, string imya, string familiya, out string errorMessage)
        {
            errorMessage = "";

            // Проверка обязательных полей (TC_REG_02)
            if (string.IsNullOrWhiteSpace(login))
            {
                errorMessage = "Логин обязателен для заполнения";
                return false;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Пароль обязателен для заполнения";
                return false;
            }
            if (string.IsNullOrWhiteSpace(imya))
            {
                errorMessage = "Имя обязательно для заполнения";
                return false;
            }
            if (string.IsNullOrWhiteSpace(familiya))
            {
                errorMessage = "Фамилия обязательна для заполнения";
                return false;
            }

            // Проверка уникальности логина (TC_REG_03)
            if (dbHelper.ProveritSushestvovanieLogina(login))
            {
                errorMessage = "Пользователь с таким логином уже существует";
                return false;
            }

            // Попытка сохранения в БД
            try
            {
                dbHelper.RegistrirovatPolzovatela(login, password, imya, familiya);
                return true;
            }
            catch
            {
                errorMessage = "Ошибка при регистрации. Попробуйте позже.";
                return false;
            }
        }

        // Обработчик кнопки "Зарегистрироваться"
        private void ButtonZaregistrirovatsya_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string parol = PasswordBoxParol.Password;
            string imya = TextBoxImya.Text;
            string familiya = TextBoxFamiliya.Text;

            if (Register(login, parol, imya, familiya, out string error))
            {
                MessageBox.Show("Вы успешно зарегистрированы.");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(error);
            }
        }
    }
}