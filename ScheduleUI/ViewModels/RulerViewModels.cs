using System;
using System.ComponentModel;
using System.Windows.Controls;
using ScheduleUI.Models;

namespace ScheduleUI.ViewModels
{
    public class RulerViewModels : INotifyPropertyChanged
    {
        private readonly RulerModel model = new();

        private double _actualWidth;
        private double _actualHeight;

        private Canvas myCanvas;
        public Canvas MyCanvas
        {
            get { return myCanvas; }
            set
            {
                if (myCanvas != value)
                {
                    myCanvas = value;
                    OnPropertyChanged(nameof(MyCanvas));
                }
            }
        }

        public RulerViewModels()
        {
            MainWindowViewModels.ButtonClicked += OnButtonClickedFromMainWindow;
        }

        private void OnButtonClickedFromMainWindow(object sender, EventArgs e)
        {
            UpdateLine(_actualWidth, _actualHeight);
        }

        public void UpdateLine(double actualWidth, double actualHeight)
        {
            if (MyCanvas != null)
            {
                MyCanvas.Children.Clear();
            }
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;
            MyCanvas = model.GetTicks(actualWidth, actualHeight);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
