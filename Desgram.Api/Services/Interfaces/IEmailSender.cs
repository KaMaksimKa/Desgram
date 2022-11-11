using Desgram.Api.Services.ServiceModel.Interfaces;

namespace Desgram.Api.Services.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(IEmailMessage message);
    }
}
