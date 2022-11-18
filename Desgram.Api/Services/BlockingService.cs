using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
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

        public BlockingService(ApplicationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task BlockUserAsync(Guid blockUserId, Guid userId)
        {

            if (await CheckBlockingAsync(blockUserId, userId))
            {
                throw new BlockingAlreadyExistsException();
            }

            var blockUser = await _context.Users.GetUserByIdAsync(blockUserId);
            var user = await _context.Users.GetUserByIdAsync(userId);

            var blockingUser = new BlockingUser()
            {
                Id = Guid.NewGuid(),
                User = user,
                DeletedDate = null,
                Blocked = blockUser,
                CreateDate = DateTimeOffset.Now.UtcDateTime
            };

            await _context.BlockingUsers.AddAsync(blockingUser);

            await _context.SaveChangesAsync();
        }

        public async Task UnblockUserAsync(Guid blockUserId, Guid userId)
        {
            var blockingUser = await GetBlockingAsync(blockUserId, userId);

            blockingUser.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task<List<PartialUserModel>> GetBlockedUsersAsync(Guid userId)
        {
            var blockedUsers = await _context.BlockingUsers
                .Where(b => b.UserId == userId && b.DeletedDate == null)
                .Select(b=>b.Blocked)
                .ProjectTo<PartialUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return blockedUsers;
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
