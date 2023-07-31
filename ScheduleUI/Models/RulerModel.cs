using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ScheduleUI.Models
{
    public class RulerModel
    {
        Canvas _canvas = new();
        public Canvas GetTicks(double controlWidth, double controlHeight)
        {
            for (int i = 0; i <= ConfigConstants.MaxLine; i++)
            {
                Line tick = new()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                double positionX = controlWidth / ConfigConstants.MaxLine * i;
                double positionY = controlHeight;
                if (i % 5 == 0)
                {
                    switch (i)
                    {
                        case 0:
                            CreateLine(tick, positionX, 0, positionY);
                            CreatText(i, positionX, positionY);
                            break;
                        case 70:
                            CreateLine(tick, positionX, 0, positionY);
                            break;
                        default:
                            CreateLine(tick, positionX, 30, positionY);
                            CreatText(i, positionX, positionY);
                            break;
                    }
                }
                else
                {
                    CreateLine(tick, positionX, 45, 50);
                }

                _canvas.Children.Add(tick);
            }

            TextBlock dateTextBlock = new()
            {
                Text = DateTime.Now.ToString("MMM d ddd HH:mm", CultureInfo.InvariantCulture),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10, 5, 0, 0),
            };

            _canvas.Children.Add(dateTextBlock);

            return _canvas;
        }

        private void CreatText(int num, double positionX, double positionY)
        {
            TextBlock textBlock = new()
            {
                Text = num.ToString(),
                Margin = new Thickness(positionX + 5, 25, 0, 0)
            };

            _canvas.Children.Add(textBlock);
        }

        private static void CreateLine(Line tick, double positionX, double positionY1, double positionY2)
        {
            tick.X1 = positionX;
            tick.Y1 = positionY1;
            tick.X2 = positionX;
            tick.Y2 = positionY2;
        }

    }
}
