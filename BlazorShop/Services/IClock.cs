namespace BlazorShop.Models
{
    public interface IClock
    {
        public DateTime GetTimeUtc();
        DateTime GetLocalTime();
    }
}
