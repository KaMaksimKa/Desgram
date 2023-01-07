using AutoMapper;
using AutoMapper.QueryableExtensions;
using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models;
using Desgram.Api.Models.Notification;
using Desgram.Api.Models.Push;
using Desgram.Api.Services.Interfaces;
using Desgram.DAL;
using Desgram.DAL.Entities;
using Desgram.SharedKernel.Exceptions.NotFoundExceptions;
using Microsoft.EntityFrameworkCore;

namespace Desgram.Api.Services
{
    public class NotificationService:INotificationService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IPushService _pushService;
        private readonly ICustomMapperService _customMapperService;

        public NotificationService(ApplicationContext context,IMapper mapper,IPushService pushService,ICustomMapperService customMapperService)
        {
            _context = context;
            _mapper = mapper;
            _pushService = pushService;
            _customMapperService = customMapperService;
        }
        public async Task CreateLikePostNotificationAsync(LikePost likePost)
        {
            var userId = (await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == likePost.PostId && p.DeletedDate == null))?.UserId;
            if (userId == null)
            {
                throw new UserNotFoundException();
            }

            await _context.Notifications.AddAsync(new Notification()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                LikePost = likePost,
                UserId = userId.Value,
            });
            await _context.SaveChangesAsync();

            await _pushService.SendPushAsync(new LikePostPushModel(likePost.User.Name),userId.Value);
        }

        public async Task CreateLikeCommentNotificationAsync(LikeComment likeComment)
        {
            var userId = (await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == likeComment.CommentId && c.DeletedDate ==null))?.UserId;
            if (userId == null)
            {
                throw new UserNotFoundException();
            }

            await _context.Notifications.AddAsync(new Notification()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                LikeComment = likeComment,
                UserId = userId.Value,
            });
            await _context.SaveChangesAsync();

            await _pushService.SendPushAsync(new LikeCommentPushModel(likeComment.User.Name,likeComment.Comment.Content), userId.Value);
        }

        public async Task CreateCommentNotificationAsync(Comment comment)
        {
            var userId = (await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == comment.PostId && p.DeletedDate == null))?.UserId;
            if (userId == null)
            {
                throw new UserNotFoundException();
            }

            await _context.Notifications.AddAsync(new Notification()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                Comment = comment,
                UserId = userId.Value,
            });
            await _context.SaveChangesAsync();

            await _pushService.SendPushAsync(new CommentPushModel(comment.User.Name, comment.Content), userId.Value);
        }

        public async Task CreateSubscriptionNotificationAsync(UserSubscription subscription)
        {

            await _context.Notifications.AddAsync(new Notification()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTimeOffset.Now.UtcDateTime,
                Subscription = subscription ,
                UserId = subscription.ContentMakerId,
            });
            await _context.SaveChangesAsync();

            if (subscription.IsApproved)
            {
                await _pushService.SendPushAsync(new SubscriptionPushModel(subscription.Follower.Name), subscription.ContentMakerId);
            }
            else
            {
                await _pushService.SendPushAsync(new SubRequestPushModel(subscription.Follower.Name), subscription.ContentMakerId);
            }
        }

        public async Task DeleteLikePostNotificationAsync(Guid likePostId)
        {
            var notification =await 
                _context.Notifications.FirstOrDefaultAsync(n => n.DeletedDate == null && n.LikePostId == likePostId);
            if (notification == null)
            {
                throw new NotificationNotFoundException();
            }
            notification.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikeCommentNotificationAsync(Guid likeCommentId)
        {
            var notification = await
                _context.Notifications.FirstOrDefaultAsync(n => n.DeletedDate == null && n.LikeCommentId == likeCommentId);
            if (notification == null)
            {
                throw new NotificationNotFoundException();
            }
            notification.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentNotificationAsync(Guid commentId)
        {
            var notification = await
                _context.Notifications.FirstOrDefaultAsync(n => n.DeletedDate == null && n.CommentId ==commentId );
            if (notification == null)
            {
                throw new NotificationNotFoundException();
            }
            notification.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionNotificationAsync(Guid subscriptionId)
        {
            var notification = await
                _context.Notifications.FirstOrDefaultAsync(n => n.DeletedDate == null && n.SubscriptionId == subscriptionId);
            if (notification == null)
            {
                throw new NotificationNotFoundException();
            }
            notification.DeletedDate = DateTimeOffset.Now.UtcDateTime;
            await _context.SaveChangesAsync();
        }


        public async Task<int> GetNumberUnseenNotificationsAsync(Guid requestorId)
        {
             return await _context.Notifications.CountAsync(n => n.DeletedDate == null && !n.HasViewed);
       
        }

        public async Task<List<NotificationModel>> GetNotificationsAsync(SkipDateTakeModel model, Guid requestorId)
        {

            var notifications =  _context.Notifications
                .Where(n => n.DeletedDate == null
                            && n.UserId == requestorId
                            && (model.SkipDate == null || n.CreatedDate < model.SkipDate.Value.UtcDateTime))
                .OrderByDescending(n => n.CreatedDate)
                .Take(model.Take);

            var notificationModels = await _customMapperService.ToNotificationModelsList(notifications);

            var notificationForChangeHasViewed = await _context.Notifications
                .Where(n => n.DeletedDate == null
                            && n.UserId == requestorId
                            && (model.SkipDate == null || n.CreatedDate < model.SkipDate.Value.UtcDateTime))
                .OrderByDescending(n => n.CreatedDate)
                .Take(model.Take)
                .ToListAsync();

            foreach (var notification in notificationForChangeHasViewed)
            {
                notification.HasViewed = true;
            }

            await _context.SaveChangesAsync();
            return notificationModels;
           
        }
    }
}
