using System;

namespace SchedulingApp.Helpers
{
    public static class TimeZoneHelper
    {
        // Converts a local DateTime to Eastern Standard Time
        public static DateTime ToEasternTime(DateTime localTime)
        {
            TimeZoneInfo eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, eastern);
        }

        // Converts a UTC DateTime to the user's local time, accounting for DST
        public static DateTime ToLocalTime(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
        }

        // Converts a local DateTime to UTC for storage
        public static DateTime ToUtcTime(DateTime localTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZoneInfo.Local);
        }

        // Returns the user's current timezone display name
        public static string GetUserTimeZoneName()
        {
            return TimeZoneInfo.Local.DisplayName;
        }

        // Checks if a given DateTime falls within DST
        public static bool IsDaylightSavingTime(DateTime time)
        {
            return TimeZoneInfo.Local.IsDaylightSavingTime(time);
        }
    }
}