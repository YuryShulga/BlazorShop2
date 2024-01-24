using BlazorShop.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BlazorShop.Services;

// Decorator pattern. Прием: перехват зависимости и добавление функциональности. Делается через Scrutor.

public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
{
    private readonly SmtpClient _client = new();
    
    private readonly ILogger<MailKitSmtpEmailSender> _logger;

    private readonly IOptions<EmailMessageAutorizationModel> _options;

    private readonly IConfiguration _configuration;

	public MailKitSmtpEmailSender(
        ILogger<MailKitSmtpEmailSender> logger,
        IOptions<EmailMessageAutorizationModel> options,
		IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options;
        _configuration = configuration;
    }
    
    public async Task SendEmail(string recieverEmail, string subject, string htmlBody, string senderName = "MyShop")
    {
        _logger.LogInformation("Sending email to {Email} with subject {Subject}", recieverEmail, subject);
        
        using var emailMessageToSend = new MimeMessage();
        emailMessageToSend.From.Add(new MailboxAddress(senderName, _configuration["Login"]));
        emailMessageToSend.To.Add(new MailboxAddress("", recieverEmail));
        emailMessageToSend.Subject = subject;
        emailMessageToSend.Body = new TextPart(TextFormat.Html)
        {
            Text = htmlBody,
        };

        await EnsureConnectedAndAuthed();
        await _client.SendAsync(emailMessageToSend);
    }

    private async Task EnsureConnectedAndAuthed()
    {
        if (!_client.IsConnected)
        {
            await _client.ConnectAsync(_options.Value.SmtpServer, _options.Value.Port, false);
        }
        if (!_client.IsAuthenticated)
        {
            await _client.AuthenticateAsync(_configuration["Login"], _configuration["Parol"]);
        }
    }

    // Вызовет DI контейнер, который вызовет DisposeAsync
    public async ValueTask DisposeAsync()
    {
        await _client.DisconnectAsync(true);
        _client.Dispose();
    }
}