namespace BlazorShop.Models;

public class FakeClock : IClock
{
    private readonly DateTime _time;

    public FakeClock(DateTime time)
    {
        _time = time;
    }

    public DateTime GetTimeUtc()
    {
        return _time;
    }

    public DateTime GetLocalTime()
    {
        return _time.ToLocalTime();
    }
}