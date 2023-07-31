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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScheduleUI.Models;

namespace VirtualCanvasDemo
{
    /// <summary>
    /// Логика взаимодействия для VirtualizedCanvas.xaml
    /// </summary>
    public partial class VirtualizedCanvas : UserControl
    {
        DemoSpatialIndex index = new DemoSpatialIndex();

        public VirtualizedCanvas()
        {
            InitializeComponent();
        }

        public void Generate(double w, double x, double y, SolidColorBrush customBrush, TimeBlock block)
        {

            Rect bounds = new Rect(x, y, w, ConfigConstants.RowHeight - 4);
            index.Insert(new DemoShape()
            {
                Bounds = bounds,
                Block = block,
                IsVisible = true,
                Fill = customBrush,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Type = (ShapeType)1,
                StarPoints = 1
            });
            index.Insert(new DemoShape()
            {
                Bounds = bounds,
                Block = block,
                IsVisible = true,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Type = (ShapeType)0,
                StarPoints = 0
            });
        }

        public void CreateCanvas(double maxX, double maxY)
        {
            index.Extent = new Rect(0, 0, maxX + 4, maxY);
            this.Diagram.Index = index;
            this.Diagram.ScrollExtent = index.Extent;


        }
    }
}
