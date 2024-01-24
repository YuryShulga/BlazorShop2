
namespace BlazorShop.Models
{
    public class RealClock : IClock
    {
        public DateTime GetTimeUtc()
        {
            return DateTime.UtcNow;
        }
        
        public DateTime GetLocalTime()
        {
            return DateTime.Now;
        }
    }
}
