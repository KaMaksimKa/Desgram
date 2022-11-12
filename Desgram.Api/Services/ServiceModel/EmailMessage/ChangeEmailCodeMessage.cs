using Desgram.Api.Services.ServiceModel.Interfaces;

namespace Desgram.Api.Services.ServiceModel.EmailMessage
{
    public class ChangeEmailCodeMessage : IEmailMessage
    {
        public string Email { get; init; }
        public string Subject { get; init; } = "Подтвердите электронный адрес для Desgram";
        public string Body { get; init; }

        public ChangeEmailCodeMessage(string email, string code)
        {
            Email = email;
            Body = GetBody(code);
        }


        private string GetBody(string code)
        {
            return $"{code} — введите, чтобы сменить электронную почту в приложении Desgram. Если почту меняете не вы, просто игнорируйте это письмо.";
        }
    }
}
