using System.Windows;

namespace PR_14.Windows
{
    public partial class LichnayaStranicaWindow : Window
    {
        private DatabaseHelper dbHelper;

        public LichnayaStranicaWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            ZagruzitInformaciyu();
        }

        private void ZagruzitInformaciyu()
        {
            var polzovatel = LoginWindow.TekushiyPolzovatel;
            if (polzovatel != null)
            {
                TextBlockPolzovatel.Text = $"{polzovatel.Imya} {polzovatel.Familiya} (Login: {polzovatel.Login})";

                var bileti = dbHelper.PoluchitBiletiPolzovatela(polzovatel.Id);
                ListViewBileti.ItemsSource = bileti;
            }
        }

        private void ButtonGlavnaya_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}