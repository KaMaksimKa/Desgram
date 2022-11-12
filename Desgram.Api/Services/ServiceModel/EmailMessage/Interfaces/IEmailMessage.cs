namespace Desgram.Api.Services.ServiceModel.Interfaces
{
    public interface IEmailMessage
    {
        public string Email { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }
    }
}
