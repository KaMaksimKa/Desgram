using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.Api.Services.ServiceModel.Subscription;
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

        public async Task SubscribeAsync(SubscriptionModel model)
        {
            var follower = await _context.Users.GetUserByIdAsync(model.FollowerId);
            var contentMaker = await _context.Users.GetUserByIdAsync(model.ContentMakerId);


            if (await CheckSubscriptionAsync(model))
            {
                throw new CustomException("you've already subscribe");
            }


            var subscribe = new UserSubscription()
            {
                Id = Guid.NewGuid(),
                Follower = follower,
                ContentMaker = contentMaker,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null,
                IsApproved = !contentMaker.IsPrivate
            };


            await _context.UserSubscriptions.AddAsync(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeAsync(SubscriptionModel model)
        {
            var subscription = await GetSubscriptionAsync(model);
            subscription.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFollowerAsync(SubscriptionModel model)
        {
            var subscription = await GetSubscriptionAsync(model);
            subscription.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task AcceptSubscriptionAsync(SubscriptionModel model)
        {
            var subscription = await GetSubscriptionAsync(model);

            subscription.IsApproved = true;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PartialUserModel>> GetFollowingAsync(Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            var following =await _context.UserSubscriptions
                .Where(s => s.FollowerId == user.Id && s.DeletedDate == null && s.IsApproved)
                .Select(s=>s.ContentMaker)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return following;
        }

        public async Task<List<PartialUserModel>> GetFollowersAsync(Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);
            
            var followers =await _context.UserSubscriptions
                .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null && s.IsApproved)
                .Select(s=>s.Follower)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return followers;

        }

        public async Task<List<PartialUserModel>> GetSubRequestsAsync(Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            var partialUserModels = await _context.UserSubscriptions
                .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null && s.IsApproved == false)
                .Select(s=>s.Follower)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return partialUserModels;
        }

        private async Task<UserSubscription> GetSubscriptionAsync(SubscriptionModel model)
        {
            var subscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.FollowerId == model.FollowerId
                                          && s.ContentMakerId == model.ContentMakerId 
                                          && s.DeletedDate == null);

            if (subscription == null)
            {
                throw new CustomException("subscription not found");
            }

            return subscription;
        }

        private async Task<bool> CheckSubscriptionAsync(SubscriptionModel model)
        {
            return await _context.UserSubscriptions
                .AnyAsync(s => s.FollowerId == model.FollowerId
                                          && s.ContentMakerId == model.ContentMakerId
                                          && s.DeletedDate == null);
        }

    }
}
