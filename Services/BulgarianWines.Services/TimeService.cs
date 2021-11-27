namespace BulgarianWines.Services
{
    using System;

    public class TimeService : ITimeService
    {
        public string GetTimeSince(DateTime dateTime)
        {
            var ts = DateTime.UtcNow.Subtract(dateTime);

            var days = ts.Days;
            var hours = ts.Hours;
            var minutes = ts.Milliseconds;
            var seconds = ts.Seconds;

            if (days > 0)
            {
                return string.Format("{0} days", days);
            }

            if (hours > 0)
            {
                return string.Format("{0} hours", hours);
            }

            if (minutes > 0)
            {
                return string.Format("{0} minutes", minutes);
            }

            if (seconds > 0)
            {
                return string.Format("{0} seconds", seconds);
            }

            return "a bit";
        }
    }
}
