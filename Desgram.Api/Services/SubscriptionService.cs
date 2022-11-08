using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Models.Subscription;
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
        private readonly IMapper _mapper;

        public SubscriptionService(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Subscribe(Guid subscriberId, string subscriptionUserName)
        {
            var subscriber = await GetUserByIdAsync(subscriberId);
            var subscription = await GetUserByNameAsync(subscriptionUserName);

            if (_context.UserSubscriptions
                .Any(s => s.SubscriberId == subscriberId && s.SubscriptionId == subscription.Id && s.DeletedDate == null))
            {
                throw new CustomException("you've already subscribe");
            }

            var subscribe = new UserSubscription()
            {
                Id = Guid.NewGuid(),
                Subscriber = subscriber,
                Subscription = subscription,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };


            await _context.UserSubscriptions.AddAsync(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task Unsubscribe(Guid subscriberId, string subscriptionUserName)
        {
            var subscriber = await GetUserByIdAsync(subscriberId);
            var subscription = await GetUserByNameAsync(subscriptionUserName);

            if (await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.SubscriberId == subscriberId
                                          && s.SubscriptionId == subscription.Id && s.DeletedDate == null) is not {} subscribe)
            {
                throw new CustomException("you've not subscribe yet");
            }


            subscribe.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubscriptionModel>> GetSubscriptions(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);

            var subscriptions =await _context.UserSubscriptions
                .Where(s => s.SubscriberId == user.Id && s.DeletedDate == null)
                .ProjectTo<SubscriptionModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return subscriptions;
        }

        public async Task<List<SubscriberModel>> GetSubscribers(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            
            var subscribers =await _context.UserSubscriptions
                .Where(s => s.SubscriptionId == user.Id && s.DeletedDate == null)
                .ProjectTo<SubscriberModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return subscribers;

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
