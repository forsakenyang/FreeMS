namespace FreeMS;

public static class DateTimeExtension
{
    public static long ToTimestamp(this DateTime datetime)
    {
        var delta = datetime - DateTime.UnixEpoch;
        return (long)delta.TotalMilliseconds;
    }
}