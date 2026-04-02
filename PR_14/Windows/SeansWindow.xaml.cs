using PR_14.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PR_14.Windows
{
    public partial class SeansWindow : Window
    {
        private int seansId;
        private int vibranoMesto;
        private DatabaseHelper dbHelper;
        private List<int> zanyatieMesta;

        public SeansWindow(int id)
        {
            InitializeComponent();
            seansId = id;
            vibranoMesto = 0;
            dbHelper = new DatabaseHelper();
            ZagruzitSeans();
        }

        private void ZagruzitSeans()
        {
            var seansi = dbHelper.PoluchitSeansiPoFilmu(0);
            foreach (var seans in seansi)
            {
                if (seans.Id == seansId)
                {
                    TextBlockInfo.Text = $"{seans.FilmNazvanie} - Zal {seans.ZalNomer} - {seans.DataSeansa:dd.MM.yyyy} {seans.Vremya:hh\\:mm}";
                    break;
                }
            }

            zanyatieMesta = dbHelper.PoluchitZanyatieMesta(seansId);
            SozdatMesta();
        }

        private void SozdatMesta()
        {
            WrapPanelMesta.Children.Clear();

            for (int i = 1; i <= 50; i++)
            {
                Button button = new Button();
                button.Content = i.ToString();
                button.Width = 40;
                button.Height = 40;
                button.Margin = new Thickness(5);
                button.Tag = i;

                if (zanyatieMesta.Contains(i))
                {
                    button.Background = System.Windows.Media.Brushes.Red;
                    button.IsEnabled = false;
                }
                else
                {
                    button.Click += ButtonMesto_Click;
                }

                WrapPanelMesta.Children.Add(button);
            }
        }

        private void ButtonMesto_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            vibranoMesto = (int)button.Tag;

            foreach (var child in WrapPanelMesta.Children)
            {
                if (child is Button btn && btn.Tag is int mesto)
                {
                    if (mesto == vibranoMesto)
                    {
                        btn.Background = System.Windows.Media.Brushes.Green;
                    }
                    else if (!zanyatieMesta.Contains(mesto))
                    {
                        btn.Background = System.Windows.Media.Brushes.LightGray;
                    }
                }
            }

            TextBlockVibranoMesto.Text = $"Выбрано место: {vibranoMesto}";
            ButtonOformit.IsEnabled = true;
        }

        private void CheckBoxSkritZanyatie_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var child in WrapPanelMesta.Children)
            {
                if (child is Button button && button.Tag is int mesto)
                {
                    if (zanyatieMesta.Contains(mesto))
                    {
                        button.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void CheckBoxSkritZanyatie_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var child in WrapPanelMesta.Children)
            {
                if (child is Button button)
                {
                    button.Visibility = Visibility.Visible;
                }
            }
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ButtonOformit_Click(object sender, RoutedEventArgs e)
        {
            Seans seans = dbHelper.PoluchitSeansPoId(seansId);
            OformlenieBiletaWindow oformlenieBiletaWindow = new OformlenieBiletaWindow(seans, vibranoMesto);
            oformlenieBiletaWindow.Show();
            this.Close();
        }
    }
}
