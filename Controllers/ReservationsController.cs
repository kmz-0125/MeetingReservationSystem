using Microsoft.AspNetCore.Mvc;

namespace MeetingReservationSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private static readonly TimeSpan JstOffset = TimeSpan.FromHours(9);

        // jst→utc
        private static DateTimeOffset ToUtcFromJst(DateTime jstDateTime)
        {
            var unspecified = DateTime.SpecifyKind(jstDateTime, DateTimeKind.Unspecified);
            var jst = new DateTimeOffset(unspecified, JstOffset);
            return jst.ToUniversalTime();
        }

        // utc→jst
        private static DateTime ToJstFromUtc(DateTimeOffset utcDateTimeOffset)
        {
            return utcDateTimeOffset.ToOffset(JstOffset).DateTime;
        }
    }
}
