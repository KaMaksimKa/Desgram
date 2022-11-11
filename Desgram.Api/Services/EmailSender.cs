using Desgram.Api.Config;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Desgram.Api.Services
{
    public class EmailSender:IEmailSender
    {
        private readonly EmailConfig _config;

        public EmailSender(IOptions<EmailConfig> config)
        {
            _config = config.Value;
        }

        public async Task SendEmailAsync(IEmailMessage message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Desgram", _config.DesgramEmailAddress));
            emailMessage.To.Add(new MailboxAddress("", message.Email));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = message.Body
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(_config.ConnectAddress, _config.ConnectPort, _config.UseSsl);
            await client.AuthenticateAsync(_config.DesgramEmailAddress, _config.DesgramEmailAddressPassword);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
