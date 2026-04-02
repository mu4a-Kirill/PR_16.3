using PR_14.Models;
using PR_14.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR_14
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;
        private List<Film> vsiFilmi;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            ZagruzitFilmi();
        }

        private void ZagruzitFilmi()
        {
            vsiFilmi = dbHelper.PoluchitVseFilmi();
            ItemsControlFilmi.ItemsSource = vsiFilmi;
        }

        private void TextBoxPoisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            string poisk = TextBoxPoisk.Text.ToLower();
            var otfiltrovannieFilmi = vsiFilmi.Where(f => f.Nazvanie.ToLower().Contains(poisk)).ToList();
            ItemsControlFilmi.ItemsSource = otfiltrovannieFilmi;
        }

        private void ComboBoxSortirovka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSortirovka.SelectedItem == null) return;

            string vibranSort = (ComboBoxSortirovka.SelectedItem as ComboBoxItem).Content.ToString();
            List<Film> sortirovannieFilmi;

            if (vibranSort == "По названию")
            {
                sortirovannieFilmi = ItemsControlFilmi.ItemsSource as List<Film>;
                if (sortirovannieFilmi != null)
                {
                    sortirovannieFilmi = sortirovannieFilmi.OrderBy(f => f.Nazvanie).ToList();
                }
            }
            else
            {
                sortirovannieFilmi = ItemsControlFilmi.ItemsSource as List<Film>;
                if (sortirovannieFilmi != null)
                {
                    sortirovannieFilmi = sortirovannieFilmi.OrderByDescending(f => f.Reyting).ToList();
                }
            }

            ItemsControlFilmi.ItemsSource = sortirovannieFilmi;
        }

        private void ButtonPodrobnee_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int filmId = (int)button.Tag;

            FilmWindow filmWindow = new FilmWindow(filmId);
            filmWindow.Show();
            this.Close();
        }

        private void ButtonLichnayaStranica_Click(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.TekushiyPolzovatel == null)
            {
                MessageBox.Show("Войти в систему");
                return;
            }

            LichnayaStranicaWindow lichnayaStranicaWindow = new LichnayaStranicaWindow();
            lichnayaStranicaWindow.Show();
            this.Close();
        }
    }
}