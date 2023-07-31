using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using ScheduleUI.Models;
using Bitlush;
using System.Collections.Generic;

namespace ScheduleUI.ViewModels
{
    public class MainWindowViewModels : INotifyPropertyChanged
    {
        private readonly BackgroundModel modelBackground = new();
        private readonly ScheduleModel modelSchedule = new();
       
        private readonly ICommand buttonCommand;

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

        private VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvasContent;

        public VirtualCanvasDemo.VirtualizedCanvas VirtualizedCanvasContent
        {
            get { return virtualizedCanvasContent; }
            set
            {
                if (virtualizedCanvasContent != value)
                {
                    virtualizedCanvasContent = value;
                    OnPropertyChanged(nameof(VirtualizedCanvasContent));
                }
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
        public VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvas;
        public void GenerateSchedule()
        {

            if (BackgroundGrid != null)
            {
                BackgroundGrid.Children.Clear();
            }

            string completed = null;
            string pending = null;
            string disabled = null;

            VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvas = new();

            modelSchedule.CreateGrid(virtualizedCanvas);

            modelSchedule.NumberOfTypes(ref pending, ref disabled, ref completed);
            PrintNumberOfTypes(completed, pending, disabled);
           
            DrawGrid(modelSchedule.Layer, virtualizedCanvas);
            VirtualizedCanvasContent = virtualizedCanvas;
        }

        public void DrawGrid(int layer, VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvas)
        {
            BackgroundGrid = modelBackground.DrawBackground(36);
            virtualizedCanvas.CreateCanvas(modelSchedule.FindMaxX(), layer * ConfigConstants.RowHeight);
        }

        public void PrintNumberOfTypes(string completed, string pending, string disabled)
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
