namespace ShriGo.Helpers
{
    public static class TimeHelper
    {
        private static readonly
        TimeZoneInfo IndiaTimeZone =

            TimeZoneInfo
                .FindSystemTimeZoneById(
                    "India Standard Time"
                );

        public static DateTime
            GetIndiaTime()
        {
            return
                TimeZoneInfo
                    .ConvertTimeFromUtc(
                        DateTime.UtcNow,
                        IndiaTimeZone
                    );
        }

        public static DateOnly
            GetIndiaDate()
        {
            return
                DateOnly
                    .FromDateTime(
                        GetIndiaTime()
                    );
        }

        public static TimeOnly
            GetIndiaTimeOnly()
        {
            return
                TimeOnly
                    .FromDateTime(
                        GetIndiaTime()
                    );
        }
    }
}