using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using VirtualCanvasDemo.Interfaces;

namespace VirtualCanvasDemo
{
    /// <summary>
    /// This is an even more light weight way to create the visuals by using drawingContext
    /// </summary>
    class DemoShapeVisual : FrameworkElement /*ISemanticZoomable*/
    {
        double scale = 1.0;

        public DemoShape Shape { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Pen pen = null;
            if (Shape.Stroke != null)
            {
                pen = new Pen(Shape.Stroke, Shape.StrokeThickness / this.scale);
            }

            switch (Shape.Type)
            {
                case ShapeType.Rect:
                    var position = new Rect(0, 0, Shape.Bounds.Width, Shape.Bounds.Height);
                    drawingContext.DrawRectangle(Shape.Fill, pen, position);
                    break;

                case ShapeType.Text:
                    {
                        FormattedText formattedText =
                            new FormattedText(
                                              Shape.Block.Description,
                                              CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              new Typeface("Arial"),
                                              12,
                                              Brushes.Black,
                                              new NumberSubstitution(),
                                              TextFormattingMode.Display,
                                               VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

                        drawingContext.DrawText(formattedText, new Point(5, 2));

                    }
                    break;
            }
        }

    }
}
