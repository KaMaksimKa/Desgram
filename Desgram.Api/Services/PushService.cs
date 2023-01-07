using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Push;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class PushService:IPushService
    {
        private readonly ApplicationContext _context;
        private readonly IGooglePushService _googlePushService;

        public PushService(ApplicationContext context,IGooglePushService googlePushService)
        {
            _context = context;
            _googlePushService = googlePushService;
        }
        public async Task SubscribePushAsync(PushTokenModel model, Guid sessionId)
        {
            var session = await _context.UserSessions.GetSessionById(sessionId);
            session.PushToken = model.Token;
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribePushAsync(Guid sessionId)
        {
            var session = await _context.UserSessions.GetSessionById(sessionId);
            session.PushToken = null;
            await _context.SaveChangesAsync();
        }
        
        public async Task SendPushAsync(IPushModel model, Guid userId)
        {
            var pushTokens =(await _context.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Sessions)
                .Where(s => s.IsActive && s.PushToken !=null).Select(s => s.PushToken)
                .ToListAsync())
                .OfType<string>()
                .ToList();
            _googlePushService.SendNotification(pushTokens, model);
        }
    }
}
