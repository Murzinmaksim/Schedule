using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Bitlush;

namespace ScheduleUI.Models
{
    public class ScheduleModel
    {

        private int _layer;

        private int _countCompleted;
        private int _countPending;
        private int _countDisabled;
        private readonly SolidColorBrush[] customBrush = { new((Color)ColorConverter.ConvertFromString("#50a31d")),
                                                           new((Color)ColorConverter.ConvertFromString("#c79300")),
                                                           new((Color)ColorConverter.ConvertFromString("#8d8cb1"))};

        private AvlTree<DateTime, TimeBlock> timeBlocks = new AvlTree<DateTime, TimeBlock>();

        private Grid grid = new();

        public Grid Start(ref int layer, ref string completed, ref string pending, ref string disabled)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            timeBlocks.Clear();
            _layer = 0;

            _countCompleted = 0;
            _countPending = 0;
            _countDisabled = 0;
           
            CreateTimeBlocks();

            foreach (AvlNode<DateTime, TimeBlock> blockNode in timeBlocks)
            {
                TimeBlock block = blockNode.Value;
                AddBlockToGrid(block);
            }

            PrintBlockCountsByType(ref pending, ref disabled, ref completed);
            layer = _layer;
            return grid;
        }

        private void CreateTimeBlocks()
        {
            Random random = new();
            var startDate = DateTime.Now;
            var num = random.Next(100, 1000);
            for (int i = 0; i < 2000; i++)
            {
                var startHour = random.Next(0, 23);
                var startMinute = random.Next(0, 59);
                var endHour = random.Next(startHour, 24);
                var endMinute = random.Next(startMinute, 60);

                var daysToAdd = random.Next(0, 4);
                var date = startDate.AddDays(daysToAdd);

                var description = $"Event {i + 1}";
                ElementType type = (ElementType)random.Next(Enum.GetValues(typeof(ElementType)).Length);
                var block = new TimeBlock(type, new DateTime(date.Year, date.Month, date.Day, startHour, startMinute, 0), new DateTime(date.Year, date.Month, date.Day, endHour, endMinute, 0), description);
                timeBlocks.Insert(block.StartTime, block);
            }
        }

        private DateTime FindMinStartTime()
        {
            DateTime minStartTime = DateTime.MaxValue;
            foreach (AvlNode<DateTime, TimeBlock> blockNode in timeBlocks)
            {
                TimeBlock block = blockNode.Value;
                if (block.StartTime < minStartTime)
                {
                    minStartTime = block.StartTime;
                }
            }
            return minStartTime;
        }

        private void AddBlockToGrid(TimeBlock block)
        {
            GetAvailableLayer(block);
            var minStartTime = FindMinStartTime();
            var x = (block.StartTime - minStartTime).TotalMinutes / 10 * ConfigConstants.MinutesInHour;
            var width = GetBlockWidth(block);

            double opacity = 1;
            SolidColorBrush customBrush = ColorType(block.Type, ref opacity);

            Rectangle rectangle = new()
            {
                Width = width,
                Height = ConfigConstants.RowHeight - 2,
                Fill = customBrush,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Opacity = opacity,
            };

            var dateTimeText = $"{block.StartTime:dd.MM.yyyy HH:mm} - {block.EndTime:HH:mm} {block.Description}";

            TextBlock textBlock = new()
            {
                Text = dateTimeText,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(7, 2, 2, 2),
                Opacity = opacity,
            };

            Grid gridItem = new()
            {
                Margin = new Thickness(x, 0, 0, 0),
            };

            gridItem.Children.Add(rectangle);
            gridItem.Children.Add(textBlock);

            gridItem.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width) });

            Grid.SetRow(gridItem, block.Layer);

            grid.Children.Add(gridItem);
        }

        private double GetBlockWidth(TimeBlock block)
        {
            var widthInMinutes = block.Duration;
            var widthInPixels = widthInMinutes / 10 * ConfigConstants.MinutesInHour;
            return widthInPixels.TotalMinutes;
        }

        private void GetAvailableLayer(TimeBlock block)
        {
            var minStartTime = FindMinStartTime();
            var x = block.StartTime.Subtract(minStartTime).TotalMinutes;
            var endX = block.EndTime.Subtract(minStartTime).TotalMinutes;

            int layer;
            for (layer = 0; layer < ConfigConstants.MaxLayers; layer++)
            {
                bool layerAvailable = true;
                foreach (AvlNode<DateTime, TimeBlock> blockNode in timeBlocks)
                {
                    TimeBlock existingBlock = blockNode.Value;
                    var existingStart = existingBlock.StartTime.Subtract(minStartTime).TotalMinutes;
                    var existingEnd = existingBlock.EndTime.Subtract(minStartTime).TotalMinutes;

                    if (existingBlock.Layer == layer &&
                        ((x >= existingStart && x < existingEnd) ||
                        (endX > existingStart && endX <= existingEnd) ||
                        (x <= existingStart && endX >= existingEnd)))
                    {
                        layerAvailable = false;
                        break;
                    }

                    if (existingBlock.Layer == layer &&
                        x <= existingStart &&
                        endX >= existingEnd)
                    {
                        layerAvailable = false;
                        break;
                    }
                }

                if (layerAvailable)
                {
                    block.Layer = layer;
                    if (layer >= _layer)
                    {
                        _layer++;
                        RowDefinition rowDefinition = new()
                        {
                            Height = new GridLength(ConfigConstants.RowHeight)
                        };
                        grid.RowDefinitions.Add(rowDefinition);
                    }
                    break;
                }
            }
        }

        private SolidColorBrush ColorType(ElementType type, ref double opacity)
        {
            switch (type)
            {
                case ElementType.Active:
                    _countCompleted++;
                    return customBrush[0];

                case ElementType.Pending:
                    _countPending++;
                    return customBrush[1];
            }
            opacity = 0.6;
            _countDisabled++;
            Random random = new();
            return customBrush[random.Next(1, 3)];
        }

        private void PrintBlockCountsByType(ref string pending, ref string jeopardy, ref string completed)
        {
            pending = $"{_countPending} Pending";
            jeopardy = $"{_countDisabled} Jeopardy";
            completed = $"{ _countCompleted} Completed";
        }
    }
}
