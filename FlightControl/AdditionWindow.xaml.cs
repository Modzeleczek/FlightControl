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

            BackgroundBitmap = new WriteableBitmap((int)BackgroundImage.Width, (int)BackgroundImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            BackgroundImage.Source = BackgroundBitmap;

            PreviewBitmap = new WriteableBitmap((int)PreviewImage.Width, (int)PreviewImage.Height,
            96, 96, PixelFormats.Bgra32, null);
            PreviewImage.Source = PreviewBitmap;

            BackgroundBitmap.Lock();
            uint* pDest = (uint*)BackgroundBitmap.BackBuffer, pEnd = pDest + BackgroundBitmap.PixelWidth * BackgroundBitmap.PixelHeight;
            uint* pMap = (uint*)mapBitmap.BackBuffer;
            uint* pRoutes = (uint*)routesBitmap.BackBuffer;
            uint* pAircrafts = (uint*)aircraftsBitmap.BackBuffer;
            for (; pDest < pEnd; ++pDest)
            {
                if (*pAircrafts != 0)
                    System.Diagnostics.Debug.Write($"{*pAircrafts} ");
                if (*pAircrafts != 0)
                    *pDest = *(pAircrafts++);//Stack three layers.
                else if (*pRoutes != 0)
                    *pDest = *(pRoutes++);
                else
                    *pDest = *(pMap++);
                //*pDest -= (128 << 24);//Subtract half of opacity (alpha channel).
            }
            BackgroundBitmap.AddDirtyRect(new Int32Rect(0, 0, BackgroundBitmap.PixelWidth, BackgroundBitmap.PixelHeight));
            BackgroundBitmap.Unlock();

            Editor = new Flight.Editor();
        }

        private void ImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                double altitude = double.Parse(AltitudeTextBox.Text),
                    velocity = double.Parse(VelocityTextBox.Text);

                PreviewBitmap.Lock();
                Editor.Route.Draw(PreviewBitmap, 0);
                Editor.AddLast(e.GetPosition(PreviewImage).X, e.GetPosition(PreviewImage).Y, velocity, altitude);
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
                Editor.ReplaceLast(e.GetPosition(PreviewImage).X, e.GetPosition(PreviewImage).Y);
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
                    MessageBox.Show("Wybierz rodzaj statku", "Błąd", MessageBoxButton.OK);
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

        protected /*virtual*/ void HelpClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wpisz prędkość i wysokość, a następnie kliknij lewy przycisk myszy, aby wstawić nowy odcinek lotu.\n" +
                "Kliknięcie prawego przycisku myszy przesuwa w wybrane miejsce koniec ostatnio dodanego odcinka.\n" +
                "Przycisk 'Cofnij' usuwa ostatni dodany odcinek.\n" +
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