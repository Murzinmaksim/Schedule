using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using ScheduleUI.Models;

namespace ScheduleUI.ViewModels
{
    public class MainWindowViewModels : INotifyPropertyChanged
    {
        private readonly BackgroundModel modelBackground = new();
        private readonly ScheduleModel modelSchedule = new();

        private ICommand buttonCommand;

        public static event EventHandler ButtonClicked;

        private Grid backgroundGrid;

        public Grid BackgroundGrid
        {
            get { return backgroundGrid; }
            set
            {
                backgroundGrid = value;
                OnPropertyChanged(nameof(BackgroundGrid));
            }
        }

        private Grid scheduleGrid;

        public Grid ScheduleGrid
        {
            get { return scheduleGrid; }
            set
            {
                scheduleGrid = value;
                OnPropertyChanged(nameof(ScheduleGrid));
            }
        }

        private string pending;

        public string Pending
        {
            get { return pending; }
            set
            {
                if (pending != value)
                {
                    pending = value;
                    OnPropertyChanged(nameof(Pending));
                }
            }
        }

        private string jeopardy;

        public string Jeopardy
        {
            get { return jeopardy; }
            set
            {
                if (jeopardy != value)
                {
                    jeopardy = value;
                    OnPropertyChanged(nameof(Jeopardy));
                }
            }
        }

        private string completed;

        public string Completed
        {
            get { return completed; }
            set
            {
                if (completed != value)
                {
                    completed = value;
                    OnPropertyChanged(nameof(Completed));
                }
            }
        }

        public MainWindowViewModels()
        {
            buttonCommand = new RelayCommand(OnButtonClick);
            GenerateSchedule();
        }

        public ICommand ButtonCommand => buttonCommand;

        private void OnButtonClick(object parameter)
        {
            ButtonClicked?.Invoke(this, EventArgs.Empty);
            GenerateSchedule();
        }

        public void GenerateSchedule()
        {
            if (ScheduleGrid != null)
            {
                ScheduleGrid.Children.Clear();
            }

            if (BackgroundGrid != null)
            {
                BackgroundGrid.Children.Clear();
            }

            int layer = 0;
            string completed = null;
            string pending = null;
            string disabled = null;

            ScheduleGrid = modelSchedule.Start(ref layer, ref completed, ref pending, ref disabled);
            PrintCountsByType(completed, pending, disabled);
            DrawBack(layer);
        }

        public void DrawBack(int layer)
        {
            BackgroundGrid = modelBackground.DrawBackground(layer);
        }

        public void PrintCountsByType(string completed, string pending, string disabled)
        {
            Completed = completed;
            Pending = pending;
            Jeopardy = disabled;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
