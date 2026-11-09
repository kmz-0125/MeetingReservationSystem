namespace MeetingReservationSystem.Extensions
{
    public static class TimeHelper
    {
        public static readonly TimeSpan JstOffset = TimeSpan.FromHours(9);

        // jst→utc
        public static DateTimeOffset ToUtcFromJst(DateTime jstDateTime)
        {
            var unspecified = DateTime.SpecifyKind(jstDateTime, DateTimeKind.Unspecified);
            var jst = new DateTimeOffset(unspecified, JstOffset);
            return jst.ToUniversalTime();
        }

        // utc→jst
        public static DateTime ToJstFromUtc(DateTimeOffset utcDateTimeOffset)
        {
            return utcDateTimeOffset.ToOffset(JstOffset).DateTime;
        }
    }
}
