using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Models.Blocked;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions;
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

        public async Task BlockUserAsync(Guid userId, string blockName)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) is not { } user
                || await _context.Users.FirstOrDefaultAsync(u=>u.Name == blockName) is not {} blockUser)
            {
                throw new CustomException("user not found");
            }

            if (await _context.BlockingUsers
                    .FirstOrDefaultAsync(b =>
                        b.UserId == userId && b.Blocked.Name == blockName && b.DeletedDate == null) != null)
            {
                throw new CustomException("this user have already blocked");
            }

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

        public async Task UnblockUserAsync(Guid userId, string unblockName)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) == null
                || await _context.Users.FirstOrDefaultAsync(u => u.Name == unblockName) == null)
            {
                throw new CustomException("user not found");
            }

            if (await _context.BlockingUsers
                    .FirstOrDefaultAsync(b =>
                        b.UserId == userId && b.Blocked.Name == unblockName && b.DeletedDate == null) 
                is not {} blockingUser)
            {
                throw new CustomException("this user haven't blocked yet");
            }

            blockingUser.DeletedDate = DateTimeOffset.Now.UtcDateTime;

            await _context.SaveChangesAsync();
        }

        public async Task<List<BlockedUserModel>> GetBlockedUsersAsync(Guid userId)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) == null)
            {
                throw new CustomException("user not found");
            }

            var blockedUsers = await _context.BlockingUsers
                .Where(b => b.UserId == userId && b.DeletedDate == null)
                .ProjectTo<BlockedUserModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return blockedUsers;
        }
    }
}
