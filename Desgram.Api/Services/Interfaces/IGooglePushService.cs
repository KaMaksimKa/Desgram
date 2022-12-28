using Desgram.Api.Models.Push;

namespace Desgram.Api.Services.Interfaces
{
    public interface IGooglePushService
    {
        public List<string> SendNotification(List<string> pushTokens, PushModel model);
    }
}
