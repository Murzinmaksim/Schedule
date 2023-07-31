using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScheduleUI.Models
{
    public class BackgroundModel
    {
        Grid grid = new();

        private readonly SolidColorBrush[] customBrush = { new((Color)ColorConverter.ConvertFromString("#9ca3ad")),
                                                           new((Color)ColorConverter.ConvertFromString("#969da8"))};
        public Grid DrawBackground(int layer)
        {
            grid.Children.Clear();
            for (int i = 0; i <= layer; i++)
            {
                RowDefinition rowDefinition = new()
                {
                    Height = new GridLength(ConfigConstants.RowHeight)
                };
                grid.RowDefinitions.Add(rowDefinition);

                Border rowBackground = new()
                {
                    Background = (i % 2 == 0) ? customBrush[0] : customBrush[1],
                };

                Grid.SetRow(rowBackground, i);
                grid.Children.Add(rowBackground);
            }
            return grid;
        }
    }
}
