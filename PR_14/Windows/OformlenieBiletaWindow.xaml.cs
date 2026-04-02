using System.Windows;
using PR_14.Models;

namespace PR_14.Windows
{
    public partial class OformlenieBiletaWindow : Window
    {
        private Seans seans;
        private int mesto;
        private DatabaseHelper dbHelper;
        public OformlenieBiletaWindow(Seans seans, int mesto)
        {
            InitializeComponent();
            this.seans = seans;
            this.mesto = mesto;
            dbHelper = new DatabaseHelper();
            ZagruzitInformaciyu();
        }

        private void ZagruzitInformaciyu()
        {
            TextBlockFilm.Text = seans.FilmNazvanie;
            TextBlockZal.Text = seans.ZalNomer.ToString();
            TextBlockDataVremya.Text = $"{seans.DataSeansa:dd.MM.yyyy} {seans.Vremya:hh\\:mm}";
            TextBlockMesto.Text = mesto.ToString();
            TextBlockCena.Text = $"{seans.Cena} руб.";
        }

        private void ButtonPodtverdit_Click(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.TekushiyPolzovatel != null)
            {
                dbHelper.KupitBilet(seans.Id, LoginWindow.TekushiyPolzovatel.Id, mesto);
                MessageBox.Show("ВЫ УСПЕШНО ПРИОБРЕЛИ НИЧЕГО! ПОЗДРАВЛЯЕМ!!!");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}