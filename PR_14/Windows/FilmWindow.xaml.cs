using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PR_14.Windows
{
    public partial class FilmWindow : Window
    {
        private int filmId;
        private DatabaseHelper dbHelper;

        public FilmWindow(int id)
        {
            InitializeComponent();
            filmId = id;
            dbHelper = new DatabaseHelper();
            ZagruzitFilm();
        }

        private void ZagruzitFilm()
        {
            var film = dbHelper.PoluchitFilmPoId(filmId);
            if (film != null)
            {
                TextBlockNazvanie.Text = film.Nazvanie;
                TextBlockOpisanie.Text = film.Opisanie;
                TextBlockReyting.Text = $"Reyting: {film.Reyting}";
                TextBlockVozrast.Text = $"Vozrastnoy reyting: {film.VozrastnoyReyting}+";

                if (!string.IsNullOrEmpty(film.Oblozhka))
                {
                    Uri uri = new Uri(film.Oblozhka, UriKind.RelativeOrAbsolute);
                    ImageFilm.Source = new BitmapImage(uri);
                }
            }

            var seansi = dbHelper.PoluchitSeansiPoFilmu(filmId);
            ListViewSeansi.ItemsSource = seansi;
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonVybratSeans_Click(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.TekushiyPolzovatel == null)
            {
                MessageBox.Show("Для покупки билета необходимо войти в систему");
                return;
            }

            Button button = sender as Button;
            int seansId = (int)button.Tag;

            SeansWindow seansWindow = new SeansWindow(seansId);
            seansWindow.Show();
            this.Close();
        }
    }
}