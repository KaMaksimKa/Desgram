using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.Subscription;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.ForbiddenExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
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

        public async Task SubscribeAsync(Guid contentMakerId, Guid requestorId)
        {
            if (contentMakerId == requestorId)
            {
                throw new InvalidUserIdException();
            }

            if (await CheckSubscriptionAsync(requestorId,contentMakerId))
            {
                throw new SubscriptionAlreadyExistsException();
            }

            var follower = await _context.Users.GetUserByIdAsync(requestorId);
            var contentMaker = await _context.Users.GetUserByIdAsync(contentMakerId);

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

        public async Task UnsubscribeAsync(Guid contentMakerId, Guid requestorId)
        {
            var subscription = await GetSubscriptionAsync(requestorId,contentMakerId);
            subscription.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFollowerAsync(Guid followerId, Guid requestorId)
        {
            var subscription = await GetSubscriptionAsync(followerId,requestorId);
            subscription.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task AcceptSubscriptionAsync(Guid followerId, Guid requestorId)
        {
            var subscription = await GetSubscriptionAsync(followerId,requestorId);

            subscription.IsApproved = true;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PartialUserModel>> GetSubRequestsAsync(SkipTakeModel model,Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            var partialUserModels = await _context.UserSubscriptions.AsNoTracking()
                .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null && s.IsApproved == false)
                .Select(s=>s.Follower)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return partialUserModels.Select(u=>_mapper.Map<PartialUserModel>(u)).ToList();
        }

        public async Task<List<PartialUserModel>> GetUserFollowingAsync(UserFollowingRequestModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(model.UserId);

            if (user.IsPrivate && model.UserId != requestorId)
            {
                throw new AccessActionException();
            }

            var followers = await _context.UserSubscriptions.AsNoTracking()
                .Where(s => s.FollowerId == user.Id && s.DeletedDate == null && s.IsApproved)
                .Select(s => s.ContentMaker)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return followers.Select(u => _mapper.Map<PartialUserModel>(u)).ToList();
        }

        public async Task<List<PartialUserModel>> GetUserFollowersAsync(UserFollowersRequestModel model, Guid requestorId)
        {
            var user = await _context.Users.GetUserByIdAsync(model.UserId);

            if (user.IsPrivate && model.UserId != requestorId)
            {
                throw new AccessActionException();
            }

            var following = await _context.UserSubscriptions.AsNoTracking()
                .Where(s => s.ContentMakerId == user.Id && s.DeletedDate == null && s.IsApproved)
                .Select(s => s.Follower)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return following.Select(u => _mapper.Map<PartialUserModel>(u)).ToList();
        }

        private async Task<UserSubscription> GetSubscriptionAsync(Guid followerId, Guid contentMakerId)
        {
            var subscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(s => s.FollowerId == followerId
                                          && s.ContentMakerId == contentMakerId
                                          && s.DeletedDate == null);

            if (subscription == null)
            {
                throw new SubscriptionNotFoundException();
            }

            return subscription;
        }

        private async Task<bool> CheckSubscriptionAsync(Guid followerId, Guid contentMakerId)
        {
            return await _context.UserSubscriptions
                .AnyAsync(s => s.FollowerId == followerId
                                          && s.ContentMakerId == contentMakerId
                                          && s.DeletedDate == null);
        }

    }
}
