using Desgram.Api.Services.Dto;

namespace Desgram.Api.Services.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(EmailMessageDto dto);
    }
}
