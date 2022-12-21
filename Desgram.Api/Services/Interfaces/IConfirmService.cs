using Desgram.Api.Models;
using Desgram.Api.Models.User;

namespace Desgram.Api.Services.Interfaces
{
    public interface IConfirmService
    {
        public Task<GuidIdModel> SendEmailCodeAsync(string email,TypesEmailCodeMessage type);
        public Task ConfirmEmailCodeAsync(EmailCodeModel model);
    }
}
