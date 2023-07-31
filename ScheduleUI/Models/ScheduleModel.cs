using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Bitlush;

namespace ScheduleUI.Models
{
    public class ScheduleModel
    {

        public int Layer { get; set; }

        private int _countCompleted;
        private int _countPending;
        private int _countDisabled;
        private static Color[] customColor = { (Color)ColorConverter.ConvertFromString("#50a31d"),
                                       (Color)ColorConverter.ConvertFromString("#c79300"),
                                       (Color)ColorConverter.ConvertFromString("#8d8cb1")};

        private readonly SolidColorBrush[] customBrush = { new(customColor[0]),
                                                           new(customColor[1])};

        private AvlTree<DateTime, TimeBlock> timeBlocks = new();

        public void CreateGrid(VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvas)
        {
            timeBlocks.Clear();
            Layer = 0;

            _countCompleted = 0;
            _countPending = 0;
            _countDisabled = 0;
           
            CreateTimeBlocks();

            foreach (AvlNode<DateTime, TimeBlock> blockNode in timeBlocks)
            {
                TimeBlock block = blockNode.Value;
                AddBlockToGrid(block, virtualizedCanvas);
            }
        }

        private void CreateTimeBlocks()
        {
            var random = new Random();
            var startDate = DateTime.Now;
            for (int i = 0; i < 2000; i++)
            {
                var startHour = random.Next(0, 23);
                var endHour = random.Next(startHour, 24);
                var startMinute = random.Next(0, 59);
                var endMinute = random.Next(startMinute, 60);

                if (endHour - startHour == 0)
                {
                    while (endMinute - startMinute < 35)
                    {
                        startMinute = random.Next(0, 59);
                        endMinute = random.Next(startMinute, 60);

                    }
                }
               
                var daysToAdd = random.Next(0, 4);
                var date = startDate.AddDays(daysToAdd);

                var startTime = new DateTime(date.Year, date.Month, date.Day, startHour, startMinute, 0);
                var endTime = new DateTime(date.Year, date.Month, date.Day, endHour, endMinute, 0);
                var description = $"{startTime:dd.MM.yyyy HH:mm} - {endTime:HH:mm} Event {i + 1}";
                ElementType type = (ElementType)random.Next(Enum.GetValues(typeof(ElementType)).Length);
                var block = new TimeBlock(type, startTime, endTime, description);
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

        public double FindMaxX()
        {
            DateTime maxEndTime = DateTime.MinValue;
            foreach (AvlNode<DateTime, TimeBlock> blockNode in timeBlocks)
            {
                TimeBlock block = blockNode.Value;

                if (block.EndTime > maxEndTime)
                {
                    maxEndTime = block.EndTime;
                }
            }
            return GetMaxX(maxEndTime);
        }

        private double GetMaxX(DateTime maxEndTime)
        {
            var minStartTime = FindMinStartTime();
            return (maxEndTime - minStartTime).TotalMinutes / 10 * ConfigConstants.MinutesInHour;
        }

        public void AddBlockToGrid(TimeBlock block, VirtualCanvasDemo.VirtualizedCanvas virtualizedCanvas)
        {
            var y =  GetAvailableLayer(block);
            var minStartTime = FindMinStartTime();
            var x1 = ((block.StartTime - minStartTime).TotalMinutes / 10 * ConfigConstants.MinutesInHour) + 2;
            var width = GetBlockWidth(block);

            SolidColorBrush customBrush = ColorType(block.Type);

            virtualizedCanvas.Generate(width, x1, y, customBrush, block);
        }

        private double GetBlockWidth(TimeBlock block)
        {
            var widthInMinutes = block.Duration;
            var widthInPixels = widthInMinutes.TotalMinutes / 10 * ConfigConstants.MinutesInHour;
            return widthInPixels;
        }

        private int GetAvailableLayer(TimeBlock block)
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
                    if (layer >= Layer)
                    {
                        Layer++;
                    }
                    break;
                }
            }
            return layer * ConfigConstants.RowHeight;
        }

        private SolidColorBrush ColorType(ElementType type)
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
            double opacity = 0.6;
            _countDisabled++;
            Random random = new();
            SolidColorBrush colorBrush = new(customColor[random.Next(1, 3)]);
            colorBrush.Opacity = opacity;
            return colorBrush;
        }

        public void NumberOfTypes(ref string pending, ref string jeopardy, ref string completed)
        {
            pending = $"{_countPending} Pending";
            jeopardy = $"{_countDisabled} Jeopardy";
            completed = $"{ _countCompleted} Completed";
        }
    }
}
