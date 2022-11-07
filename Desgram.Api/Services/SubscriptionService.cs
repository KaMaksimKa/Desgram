using Desgram.Api.Models;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class SubscriptionService:ISubscriptionService
    {
        private readonly ApplicationContext _context;

        public SubscriptionService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Subscribe(Guid subscriberId, string subscriptionUserName)
        {
            var subscriber = await GetUserByIdAsync(subscriberId);
            var subscription = await GetUserByNameAsync(subscriptionUserName);

            if (_context.UserSubscriptions
                .Any(s => s.SubscriberId == subscriberId && s.SubscriptionId == subscription.Id))
            {
                throw new CustomException("you've already subscribe");
            }

            var subscribe = new UserSubscription()
            {
                Id = Guid.NewGuid(),
                Subscriber = subscriber,
                Subscription = subscription,
                CreateDate = DateTimeOffset.Now.UtcDateTime
            };

            subscriber.AmountSubscriptions++;
            subscription.AmountSubscribers++;

            await _context.UserSubscriptions.AddAsync(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(Guid subscriberId, string subscriptionUserName)
        {
            var subscriber = await GetUserByIdAsync(subscriberId);
            var subscription = await GetUserByNameAsync(subscriptionUserName);

            if (await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.SubscriberId == subscriberId
                                          && s.SubscriptionId == subscription.Id) is not {} subscribe)
            {
                throw new CustomException("you've not subscribe yet");
            }


            subscriber.AmountSubscriptions--;
            subscription.AmountSubscribers--;

            _context.UserSubscriptions.Remove(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubscriptionModel>> GetSubscriptions(Guid userId)
        {
            var user =await _context.Users
                .Include(u => u.Subscriptions)!
                .ThenInclude(s=>s.Subscription)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user.Subscriptions.Select(s => new SubscriptionModel()
            {
                UserName = s.Subscription.Name
            }).ToList();
        }

        public async Task<List<SubscriptionModel>> GetSubscribers(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Subscribers)!
                .ThenInclude(s=>s.Subscriber)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user.Subscribers.Select(s => new SubscriptionModel()
            {
                UserName = s.Subscriber.Name
            }).ToList();
        }

        private async Task<User> GetUserByIdAsync(Guid id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new CustomException("user not found");
            }

            return user;
        }

        private async Task<User> GetUserByNameAsync(string name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                throw new CustomException("user not found");
            }
            return user;
        }
    }
}
