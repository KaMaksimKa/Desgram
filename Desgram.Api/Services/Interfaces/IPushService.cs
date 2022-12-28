using Desgram.Api.Models.Push;

namespace Desgram.Api.Services.Interfaces
{
    public interface IPushService
    {
        public Task SubscribePushAsync(PushTokenModel model,Guid sessionId);
        public Task UnsubscribePushAsync(Guid sessionId);
        public Task SendPushAsync(PushModel model, Guid userId);
    }
}
