using System;
using System.Windows;
using System.Windows.Media;

namespace ScheduleUI.Models
{
    public class YellowStripeModel
    {
        public PointCollection GetYellowStripePoints(double actualWidth, double actualHeight)
        {

            DateTime now = DateTime.Now;
            int currentHour = now.Hour;
            int currentMinute = now.Minute;

            double coefficient1 = ConfigConstants.MaxLine / 1440.0;
            double coefficient2 = ((currentHour * 60) + currentMinute) * coefficient1;

            double stripeX = actualWidth / ConfigConstants.MaxLine * coefficient2;

            PointCollection points = new()
            {
                new Point(stripeX - 8, 1),
                new Point(stripeX + 8, 1),
                new Point(stripeX + 1, 7),
                new Point(stripeX + 1, actualHeight - 17),
                new Point(stripeX - 1, actualHeight - 17),
                new Point(stripeX - 1, 7),
            };

            return points;
        }
    }
}
