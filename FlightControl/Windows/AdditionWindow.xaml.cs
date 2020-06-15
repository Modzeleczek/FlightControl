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
        private Flight.Editor Editor;

        unsafe public AdditionWindow(WriteableBitmap mapBitmap, WriteableBitmap routesBitmap, WriteableBitmap aircraftsBitmap)
        {
            InitializeComponent();

            TypeComboBox.Items.Add("Samolot");
            TypeComboBox.Items.Add("Helikopter");
            TypeComboBox.Items.Add("Szybowiec");
            TypeComboBox.Items.Add("Balon");

            BackgroundBitmap = new WriteableBitmap(mapBitmap.PixelWidth, mapBitmap.PixelHeight,
                96, 96, PixelFormats.Bgra32, null);
            BackgroundImage.Source = BackgroundBitmap;

            PreviewBitmap = new WriteableBitmap(mapBitmap.PixelWidth, mapBitmap.PixelHeight,
            96, 96, PixelFormats.Bgra32, null);
            PreviewImage.Source = PreviewBitmap;

            uint* pDest = (uint*)BackgroundBitmap.BackBuffer, pEnd = pDest + BackgroundBitmap.PixelWidth * BackgroundBitmap.PixelHeight;
            uint* pAircrafts = (uint*)aircraftsBitmap.BackBuffer;
            uint* pRoutes = (uint*)routesBitmap.BackBuffer;
            uint* pMap = (uint*)mapBitmap.BackBuffer;
            for (; pDest < pEnd; ++pDest)
            {
                //Nakładamy na siebie 3 warstwy bitmap.
                *pDest = *pAircrafts;
                if (*pDest == 0)
                    *pDest = *pRoutes;
                if(*pDest == 0)
                    *pDest = *pMap;
                ++pAircrafts;
                ++pRoutes;
                ++pMap;
            }
            BackgroundBitmap.Lock();
            BackgroundBitmap.AddDirtyRect(new Int32Rect(0, 0, BackgroundBitmap.PixelWidth, BackgroundBitmap.PixelHeight));
            BackgroundBitmap.Unlock();

            Editor = new Flight.Editor();
        }

        private void ImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int altitude = int.Parse(AltitudeTextBox.Text);
                double velocity = double.Parse(VelocityTextBox.Text);

                PreviewBitmap.Lock();
                Editor.Route.Draw(PreviewBitmap, 0);
                var position = e.GetPosition(PreviewImage);
                Editor.AddLast(position.X, position.Y, velocity, altitude);
                Editor.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
                PreviewBitmap.Unlock();
            }
            catch (FormatException)
            {
                MessageBox.Show($"Nieprawidłowy format wysokości lub prędkości.", "Błąd", MessageBoxButton.OK);
            }
        }

        private void ImageRightClick(object sender, MouseButtonEventArgs e)
        {
            if (Editor.Count > 0)
            {
                PreviewBitmap.Lock();
                Editor.Route.Draw(PreviewBitmap, 0);
                var position = e.GetPosition(PreviewImage);
                Editor.ReplaceLast(position.X, position.Y);
                Editor.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
                PreviewBitmap.Unlock();
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if (Editor.Count == 0)
            {
                MessageBox.Show("Trasa jest pusta. Stwórz trasę lub wybierz 'Anuluj'.", "Błąd", MessageBoxButton.OK);
                return;
            }
            switch (TypeComboBox.SelectedIndex)
            {
                case 0:
                    Result = new Plane(Editor.Route);
                    break;
                case 1:
                    Result = new Helicopter(Editor.Route);
                    break;
                case 2:
                    Result = new Glider(Editor.Route);
                    break;
                case 3:
                    Result = new Balloon(Editor.Route);
                    break;
                default:
                    MessageBox.Show("Wybierz rodzaj statku.", "Błąd", MessageBoxButton.OK);
                    return;
            }
            Editor = null;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Editor = null;
            Close();
        }

        private void UndoClick(object sender, RoutedEventArgs e)
        {
            if (Editor.Count > 0)
            {
                PreviewBitmap.Lock();
                Editor.Route.Draw(PreviewBitmap, 0);
                Editor.Undo();
                Editor.Route.Draw(PreviewBitmap, (255 << 24) | (255 << 16));
                PreviewBitmap.Unlock();
            }
        }

        protected void HelpClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wpisz prędkość i wysokość, a następnie kliknij lewy przycisk myszy, aby wstawić początek nowego odcinka. " +
                "Następnie kliknij ponownie, aby wstawić nowy odcinek.\n" +
                "Kliknięcie prawego przycisku myszy przesuwa w wybrane miejsce koniec ostatnio dodanego odcinka.\n" +
                "Przycisk 'Cofnij' cofa ostatnią wykonaną operację.\n" +
                "Przycisk 'Zatwierdź' powoduje zamknięcie edytora i dodanie statku do symulacji.\n",
                "Pomoc", MessageBoxButton.OK);
        }

        public void Dispose()
        {
            PreviewImage.MouseLeftButtonDown -= ImageLeftClick;
            PreviewImage.MouseRightButtonDown -= ImageRightClick;
            ConfirmButton.Click -= ConfirmClick;
            CancelButton.Click -= CancelClick;
            UndoButton.Click -= UndoClick;
            HelpButton.Click -= HelpClick;
            Editor = null;
        }
    }
}