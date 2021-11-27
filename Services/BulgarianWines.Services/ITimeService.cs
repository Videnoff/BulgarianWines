namespace BulgarianWines.Services
{
    using System;

    public interface ITimeService
    {
        public string GetTimeSince(DateTime dateTime);
    }
}
