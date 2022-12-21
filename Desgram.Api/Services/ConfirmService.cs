using Desgram.Api.Models;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.EmailMessage;
using Desgram.Api.Services.ServiceModel.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class ConfirmService : IConfirmService
    {
        private readonly ApplicationContext _context;
        private readonly IEmailSender _emailSender;

        public ConfirmService(ApplicationContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }


        public async Task<GuidIdModel> SendEmailCodeAsync(string email, TypesEmailCodeMessage type)
        {
            var code = CodeGenerator.GetCode(6);

            switch (type)
            {
                case TypesEmailCodeMessage.ChangeEmailMessage:
                    await _emailSender.SendEmailAsync(new ChangeEmailCodeMessage(email, code));
                    break;
                case TypesEmailCodeMessage.SingUpMessage:
                    await _emailSender.SendEmailAsync(new SingUpCodeMessage(email, code));
                    break;
            }


            var emailCode = await _context.EmailCodes.AddAsync(new EmailCode()
            {
                Id = Guid.NewGuid(),
                Email = email,
                CodeHash = HashHelper.GetHash(code),
                ExpiredDate = DateTimeOffset.Now.AddMinutes(5).UtcDateTime,
                DeletedDate = null
            });
            await _context.SaveChangesAsync();

            return new GuidIdModel(){Id = emailCode.Entity.Id };
        }

        public async Task ConfirmEmailCodeAsync(EmailCodeModel model)
        {
            var emailCode = await GetEmailCodeById(model.Id);

            if (emailCode.ExpiredDate < DateTimeOffset.Now)
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(model.Code)] = new List<string>()
                    {
                        "Прошло слишком много времени. Можно запросить новый."
                    }
                });
            }

            if (!HashHelper.Verify(model.Code, emailCode.CodeHash))
            {
                throw new BadRequestException(new Dictionary<string, List<string>>()
                {
                    [nameof(model.Code)] = new List<string>()
                    {
                        "Код недействителен. Можно запросить новый."
                    }
                });
            }


        }

        private async Task<EmailCode> GetEmailCodeById(Guid id)
        {
            var emailCode = await _context.EmailCodes.FirstOrDefaultAsync(c => c.DeletedDate == null && c.Id == id);
            if (emailCode == null)
            {
                throw new EmailCodeNotFoundException();
            }

            return emailCode;
        }
    }

    public enum TypesEmailCodeMessage
    {
        SingUpMessage,
        ChangeEmailMessage
    }
}
