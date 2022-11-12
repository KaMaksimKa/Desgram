using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
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

        public async Task SubscribeAsync(Guid followerId, string contentMakerName)
        {
            var follower = await _context.Users.GetUserByIdAsync(followerId);
            var contentMaker = await _context.Users.GetUserByNameAsync(contentMakerName);

            if (contentMaker.IsPrivate)
            {
                throw new CustomException("you can't subscribe on private account");
            }

            if (_context.UserSubscriptions
                .Any(s => s.FollowerId == followerId && s.ContentMakerId == contentMaker.Id
                                                     && s.DeletedDate == null))
            {
                throw new CustomException("you've already subscribe");
            }

            var subscribe = new UserSubscription()
            {
                Id = Guid.NewGuid(),
                Follower = follower,
                ContentMaker = contentMaker,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };


            await _context.UserSubscriptions.AddAsync(subscribe);
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeAsync(Guid followerId, string contentMakerName)
        {
            var contentMaker = await _context.Users.GetUserByNameAsync(contentMakerName);

            if (await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.FollowerId == followerId
                                          && s.ContentMakerId == contentMaker.Id && s.DeletedDate == null) is not {} subscription)
            {
                throw new CustomException("you've not subscribe yet");
            }


            subscription.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            
            await _context.SaveChangesAsync();
        }

        public async Task SendSubscriptionRequest(Guid followerId, string contentMakerName)
        {
            if (await _context.SubscriptionRequests.AnyAsync(r =>
                    r.FollowerId == followerId && r.ContentMaker.Name == contentMakerName 
                                               && r.DeletedDate == null))
            {
                throw new CustomException("you've already send this subscription request");
            }

            var follower =await _context.Users.GetUserByIdAsync(followerId);
            var contentMaker = await _context.Users.GetUserByNameAsync(contentMakerName);

            var subscriptionRequest = new SubscriptionRequest()
            {
                Id = Guid.NewGuid(),
                Follower = follower,
                ContentMaker = contentMaker,
                CreateDate = DateTimeOffset.Now.UtcDateTime,
                DeletedDate = null
            };

            await _context.SubscriptionRequests.AddAsync(subscriptionRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionRequest(Guid followerId, string contentMakerName)
        {
            var subscriptionRequest = await _context.SubscriptionRequests
                .FirstOrDefaultAsync(r => 
                    r.FollowerId == followerId && r.ContentMaker.Name == contentMakerName 
                                               && r.DeletedDate == null);
            if (subscriptionRequest == null)
            {
                throw new CustomException("you haven't send this subscription request yet");
            }

            subscriptionRequest.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task RespondSubscriptionRequest(bool response,Guid requestId, Guid userId)
        {
            var request =await _context.SubscriptionRequests
                .FirstOrDefaultAsync(r => r.Id == requestId && r.DeletedDate == null);

            if (request == null)
            {
                throw new CustomException("request not found");
            }

            if (request.ContentMakerId != userId)
            {
                throw new CustomException("you don't have enough rights");
            }

            request.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            var contentMaker = await _context.Users.GetUserByIdAsync(userId);
            var follower = await _context.Users.GetUserByIdAsync(request.FollowerId);

            if (response)
            {
                var userSubscription = new UserSubscription()
                {
                    Id = Guid.NewGuid(),
                    ContentMaker = contentMaker,
                    Follower = follower,
                    CreateDate = DateTimeOffset.Now.UtcDateTime,
                    DeletedDate = null
                };

                await _context.UserSubscriptions.AddAsync(userSubscription);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<FollowingModel>> GetFollowingAsync(Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);

            var following =await _context.UserSubscriptions
                .Where(s => s.FollowerId == user.Id && s.DeletedDate == null)
                .ProjectTo<FollowingModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return following;
        }

        public async Task<List<FollowerModel>> GetFollowersAsync(Guid userId)
        {
            var user = await _context.Users.GetUserByIdAsync(userId);
            
            var followers =await _context.UserSubscriptions
                .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null)
                .ProjectTo<FollowerModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return followers;

        }

    }
}
