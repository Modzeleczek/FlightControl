using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightControl
{
    public partial class AdditionWindow : Window, IDisposable
    {
        public Aircraft Result { get; private set; }
        protected WriteableBitmap BackgroundBitmap, PreviewBitmap;
        private Flight.Builder Builder;

        public AdditionWindow()
        {
            InitializeComponent();

            Builder = new Flight.Builder();

            TypeComboBox.Items.Add("Samolot");
            TypeComboBox.Items.Add("Helikopter");
            TypeComboBox.Items.Add("Szybowiec");
            TypeComboBox.Items.Add("Balon");

            PreviewBitmap = new WriteableBitmap((int)PreviewImage.Width, (int)PreviewImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            PreviewImage.Source = PreviewBitmap;
        }

        private void ImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                double altitude = double.Parse(AltitudeTextBox.Text),
                    velocity = double.Parse(VelocityTextBox.Text);

                double x = e.GetPosition(PreviewImage).X,
                       y = e.GetPosition(PreviewImage).Y;

                PreviewBitmap.Lock();
                Builder.Route.Draw(PreviewBitmap, 0);

                Builder.Add(x, y, velocity, altitude);

                Builder.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
                PreviewBitmap.Unlock();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(
                    $"Nieprawidłowy format wysokości lub prędkości.\nSzczegóły: {ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK);
            }
        }

        private void ImageRightClick(object sender, MouseButtonEventArgs e)
        {
            PreviewBitmap.Lock();
            Builder.Route.Draw(PreviewBitmap, 0);

            double x = e.GetPosition(PreviewImage).X,
                   y = e.GetPosition(PreviewImage).Y;

            Builder.ReplaceLast(x, y);

            Builder.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
            PreviewBitmap.Unlock();
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if (Builder.Count == 0)
            {
                MessageBox.Show("Trasa jest pusta. Stwórz trasę lub wybierz 'Anuluj'.", "Błąd", MessageBoxButton.OK);
                return;
            }
            switch (TypeComboBox.SelectedIndex)
            {
                case 0:
                    Result = new Plane(Builder.Route);
                    break;
                case 1:
                    Result = new Helicopter(Builder.Route);
                    break;
                case 2:
                    Result = new Glider(Builder.Route);
                    break;
                case 3:
                    Result = new Balloon(Builder.Route);
                    break;
                default:
                    MessageBox.Show("Wybierz rodzaj statku", "Błąd", MessageBoxButton.OK);
                    return;
            }
            Builder = null;
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Builder = null;
            this.Close();
        }

        private void UndoClick(object sender, RoutedEventArgs e)
        {
            PreviewBitmap.Lock();
            Builder.Route.Draw(PreviewBitmap, 0);
            Builder.RemoveLast();
            Builder.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
            PreviewBitmap.Unlock();
        }

        protected /*virtual*/ void HelpClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wpisz prędkość i wysokość, a następnie kliknij lewy przycisk myszy, aby wstawić nowy odcinek lotu.\n" +
                "Kliknięcie prawego przycisku myszy przesuwa w wybrane miejsce koniec ostatnio dodanego odcinka.\n" +
                "Przycisk 'Cofnij' usuwa ostatni dodany odcinek.\n" +
                "Przycisk 'Zatwierdź' powoduje zamknięcie edytora i dodanie statku do symulacji.\n",
                "Pomoc",
                MessageBoxButton.OK);
        }

        public void Dispose()
        {
            PreviewImage.MouseLeftButtonDown -= ImageLeftClick;
            PreviewImage.MouseRightButtonDown -= ImageRightClick;
            ConfirmButton.Click -= ConfirmClick;
            CancelButton.Click -= CancelClick;
            UndoButton.Click -= UndoClick;
            HelpButton.Click -= HelpClick;
            Builder = null;
        }
    }
}