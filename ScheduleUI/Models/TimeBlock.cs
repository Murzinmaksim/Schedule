using System;

namespace ScheduleUI.Models
{
    public class TimeBlock
    {
        public ElementType Type { get; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int Layer { get; set; }

        public TimeBlock(ElementType type, DateTime startTime, DateTime endTime, string description)
        {
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;
            Layer = -1;
        }

        public TimeSpan Duration => EndTime - StartTime;
    }
}
