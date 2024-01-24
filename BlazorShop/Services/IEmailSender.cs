namespace BlazorShop.Models
{
    public interface IEmailSender
    {
        public Task SendEmail(string recieverEmail, string subject, string htmlBody, string senderName = "");
        public Task<string> SendEmailApi(string recieverEmail, string subject, string htmlBody, string senderName = "MyShop");

    }
}
