using Desgram.Api.Services.ServiceModel.Interfaces;

namespace Desgram.Api.Services.ServiceModel.EmailMessage
{
    public class SingUpCodeMessage : IEmailMessage
    {
        public string Email { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }

        public SingUpCodeMessage(string email, string code)
        {
            Email = email;
            Subject = GetSubject(code);
            Body = GetBody(code, email);
        }

        private string GetSubject(string code)
        {
            return $"{code} is your Desgram code";
        }

        private string GetBody(string code, string email)
        {
            return $"Hi,\r\n\r\nSomeone tried to sign up for an Desgram account with {email}. If it was you, enter this confirmation code in the app:\r\n\r\n{code}";
        }
    }
}
