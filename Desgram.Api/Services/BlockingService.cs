using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.User;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.BadRequestExceptions;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class BlockingService:IBlockingService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;

        public BlockingService(ApplicationContext context,IMapper mapper
            ,ISubscriptionService subscriptionService,ILikeService likeService
            ,ICommentService commentService)
        {
            _context = context;
            _mapper = mapper;
            _subscriptionService = subscriptionService;
            _likeService = likeService;
            _commentService = commentService;
        }


        public async Task BlockUserAsync(Guid userId, Guid requestorId)
        {
            if (userId == requestorId)
            {
                throw new InvalidUserIdException();
            }

            if (await CheckBlockingAsync(userId, requestorId))
            {
                throw new BlockingAlreadyExistsException();
            }

            var blockUser = await _context.Users.GetUserByIdAsync(userId);
            var user = await _context.Users.GetUserByIdAsync(requestorId);

            var blockingUser = new BlockingUser()
            {
                Id = Guid.NewGuid(),
                User = user,
                DeletedDate = null,
                Blocked = blockUser,
                CreateDate = DateTimeOffset.Now.UtcDateTime
            };

            await _context.BlockingUsers.AddAsync(blockingUser);

            await _commentService.DeleteAllCommentsFromUserAsync(userId, requestorId);
            await _likeService.DeleteAllLikesPostFromUserAsync(userId, requestorId);
            await _subscriptionService.DeleteMutualSubscriptionAsync(userId, requestorId);


            await _context.SaveChangesAsync();
        }

        public async Task UnblockUserAsync(Guid userId, Guid requestorId)
        {
            var blockingUser = await GetBlockingAsync(userId, requestorId);

            blockingUser.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PartialUserModel>> GetBlockedUsersAsync(SkipTakeModel model,Guid requestorId)
        {
            var blockedUsers = await _context.BlockingUsers.AsNoTracking()
                .Where(b => b.UserId == requestorId && b.DeletedDate == null)
                .Select(b=>b.Blocked)
                .Skip(model.Skip).Take(model.Take)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

      

            return blockedUsers.Select(u=> _mapper.Map<PartialUserModel>(u)).ToList();
        }

        private async Task<BlockingUser> GetBlockingAsync(Guid blockUserId, Guid userId)
        {
            var blocking = await _context.BlockingUsers
                .FirstOrDefaultAsync(b => b.UserId == userId
                                          && b.BlockedId == blockUserId
                                          && b.DeletedDate == null);
            if (blocking == null)
            {
                throw new BlockingNotFoundException();
            }

            return blocking;
        }

        private async Task<bool> CheckBlockingAsync(Guid blockUserId, Guid userId)
        {
            return await _context.BlockingUsers
                .AnyAsync(b => b.UserId == userId
                                          && b.BlockedId == blockUserId
                                          && b.DeletedDate == null);
        }
    }
}
