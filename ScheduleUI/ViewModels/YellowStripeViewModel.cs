using System;
using System.ComponentModel;
using System.Windows.Media;
using ScheduleUI.Models;


namespace ScheduleUI.ViewModels
{
    public class YellowStripeViewModel : INotifyPropertyChanged
    {
        private readonly YellowStripeModel model = new();

        private double _actualWidth;
        private double _actualHeight;

        private PointCollection polygonPoints;
        public PointCollection PolygonPoints
        {
            get { return polygonPoints; }
            set
            {
                if (polygonPoints != value)
                {
                    polygonPoints = value;
                    OnPropertyChanged(nameof(PolygonPoints));
                }
            }
        }

        public YellowStripeViewModel()
        {
            MainWindowViewModels.ButtonClicked += OnButtonClickedFromMainWindow;
        }

        private void OnButtonClickedFromMainWindow(object sender, EventArgs e)
        {
            UpdateStripe(_actualWidth, _actualHeight);
        }

        public void UpdateStripe(double actualWidth, double actualHeight)
        {
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;
            PolygonPoints = model.GetYellowStripePoints(actualWidth, actualHeight);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
