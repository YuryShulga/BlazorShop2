namespace BlazorShop.Models
{
    public class EmailMessageAutorizationModel
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }

        public EmailMessageAutorizationModel() { }

        public EmailMessageAutorizationModel(string smtpServer, string login, string parol, int port)
        {
            SmtpServer = smtpServer;
            Port = port;
        }
    }
}
