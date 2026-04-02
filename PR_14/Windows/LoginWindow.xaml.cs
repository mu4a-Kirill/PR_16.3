using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PR_14.Models;

namespace PR_14.Windows
{
    public partial class LoginWindow : Window
    {
        public static Polzovatel TekushiyPolzovatel;
        private DatabaseHelper dbHelper;
        private int _failedAttempts = 0;
        private string _currentCaptcha = "";
        public string LastErrorMessage { get; private set; } = "";

        public LoginWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        public bool Auth(string login, string password, string captchaInput = null)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                LastErrorMessage = "Введите логин и пароль!";
                return false;
            }

            if (_failedAttempts >= 3)
            {
                if (string.IsNullOrEmpty(captchaInput) || captchaInput != _currentCaptcha)
                {
                    LastErrorMessage = "Неверный код с картинки";
                    return false;
                }
            }

            var user = dbHelper.ProveritPolzovatela(login, password);
            if (user == null)
            {
                LastErrorMessage = "Неверный логин или пароль";
                _failedAttempts++;
                if (_failedAttempts >= 3) GenerateAndShowCaptcha();
                return false;
            }

            TekushiyPolzovatel = user;
            _failedAttempts = 0;
            LastErrorMessage = "";
            HideCaptcha();
            return true;
        }

        public void GenerateAndShowCaptcha()
        {
            Random rand = new Random();
            _currentCaptcha = rand.Next(1000, 9999).ToString();
            CaptchaImage.Source = GenerateCaptchaImage(_currentCaptcha);
            CaptchaPanel.Visibility = Visibility.Visible;
        }

        private ImageSource GenerateCaptchaImage(string text)
        {
            int width = 120, height = 40;
            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(Brushes.LightGray, null, new Rect(0, 0, width, height));
                var formatedText = new FormattedText(
                    text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    20,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(drawingVisual).PixelsPerDip);
                context.DrawText(formatedText, new Point(20, 8));

                Random rnd = new Random();
                for (int i = 0; i < 20; i++)
                {
                    context.DrawRectangle(Brushes.Gray, null,
                        new Rect(rnd.Next(width), rnd.Next(height), 1, 1));
                }
                for (int i = 0; i < 5; i++)
                {
                    context.DrawLine(new Pen(Brushes.DarkGray, 1),
                        new Point(rnd.Next(width), rnd.Next(height)),
                        new Point(rnd.Next(width), rnd.Next(height)));
                }
            }
            var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);
            return bitmap;
        }

        public void HideCaptcha()
        {
            CaptchaPanel.Visibility = Visibility.Collapsed;
            CaptchaTextBox.Clear();
            _currentCaptcha = "";
        }

        public void RefreshCaptcha()
        {
            GenerateAndShowCaptcha();
        }

        private void RefreshCaptcha_Click(object sender, RoutedEventArgs e)
        {
            RefreshCaptcha();
        }

        private void ButtonVhod_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = PasswordBoxParol.Password;
            string captcha = CaptchaTextBox.Text;

            if (Auth(login, password, captcha))
            {
                MessageBox.Show("Пользователь успешно найден!");
                TextBoxLogin.Clear();
                PasswordBoxParol.Clear();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(LastErrorMessage);
                if (_failedAttempts < 3) PasswordBoxParol.Clear();
                if (_failedAttempts >= 3) CaptchaTextBox.Clear();
            }
        }

        private void ButtonRegistraciya_Click(object sender, RoutedEventArgs e)
        {
            RegistraciyaWindow registraciyaWindow = new RegistraciyaWindow();
            registraciyaWindow.Show();
            this.Close();
        }

        // Методы для тестирования
        public string GetCurrentCaptcha() => _currentCaptcha;
        public int GetFailedAttempts() => _failedAttempts;
        public void ResetFailedAttempts() => _failedAttempts = 0;
    }
}