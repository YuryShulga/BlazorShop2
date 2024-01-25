using BlazorShop.Models;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BlazorShop.Services;

// Decorator pattern. Прием: перехват зависимости и добавление функциональности. Делается через Scrutor.

public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
{
    private readonly SmtpClient _client = new();
    
    private readonly ILogger<MailKitSmtpEmailSender> _logger;

    //private readonly IOptions<EmailMessageAutorizationModel> _options;

    private readonly IConfiguration _configuration;
    private int _attemptToSend = 0;

    public MailKitSmtpEmailSender(
        ILogger<MailKitSmtpEmailSender> logger,
        //IOptions<EmailMessageAutorizationModel> options,
		IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //_options = options;
        _configuration = configuration;
    }
    
    public async Task SendEmail(string receiverEmail, string subject, string htmlBody, string senderName = "MyShop")
    {
        _logger.LogInformation("Sending email to {Email} with subject {Subject}", receiverEmail, subject);
        
        using var emailMessageToSend = new MimeMessage();
        emailMessageToSend.From.Add(new MailboxAddress(senderName, _configuration["Login"]));
        emailMessageToSend.To.Add(new MailboxAddress("", receiverEmail));
        emailMessageToSend.Subject = subject;
        emailMessageToSend.Body = new TextPart(TextFormat.Html)
        {
            Text = htmlBody,
        };

        await EnsureConnectedAndAuthed();
        await _client.SendAsync(emailMessageToSend);
    }

    public async Task<string> SendEmailApi(string receiverEmail, string subject, string htmlBody, string senderName = "MyShop")
    {

        _attemptToSend++;
        _logger.LogInformation("Попытка отправки имейла на адрес {Email}", receiverEmail);

        try //Вариант 1.
        {
            await SendEmail(receiverEmail, subject, htmlBody, senderName);
            return "письмо отправлено";
        }
        catch (Exception e) when (_attemptToSend == 1
                                  && e is ServiceNotAuthenticatedException
                                        or ServiceNotConnectedException
                                        //...
                                        )
        {
            _logger.LogWarning(e, "Ошибка отправки имейла на адрес {Email}. Делаем еще одну попытку", receiverEmail, e.Message);
            await SendEmailApi(receiverEmail, subject, htmlBody, senderName); //retry
            return "Ошибка отправки письма";
        }
        catch (Exception e) // Если это последняя попытка, то логируем ошибку и выводим сообщение об ошибке.
        {
            // Даем разработчику явно понять, что произошла ошибка, и что нужно что-то делать.
            _logger.LogError(e, "Ошибка отправки имейла на адрес {Email}. Ошибка: {Error}", receiverEmail, e.Message);
            return  "Ошибка отправки письма";
        }
    }

    private async Task EnsureConnectedAndAuthed()
    {
        if (!_client.IsConnected)
        {
//            await _client.ConnectAsync(_options.Value.SmtpServer, _options.Value.Port, false);
            await _client.ConnectAsync(_configuration["SmtpServer"], int.Parse(_configuration["Port"]), false);
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