﻿using Desgram.Api.Infrastructure.Extensions;
using Desgram.Api.Models.Comment;
using Desgram.Api.Models.Post;
using Desgram.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desgram.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Api")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostController:ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public PostController(IPostService postService,
            ICommentService commentService,ILikeService likeService)
        {
            _postService = postService;
            _commentService = commentService;
            _likeService = likeService;
        }


        [HttpGet]
        public async Task<List<PostModel>> GetPosts([FromQuery]PostRequestModel model)
        {
            return await _postService.GetAllPostsAsync(model,User.GetUserId());
        }

        [Route("{hashTag}")]
        [HttpGet]
        public async Task<List<PostModel>> GetPostsByHashTag([FromQuery]PostByHashtagRequestModel model)
        {
            return await _postService.GetPostByHashTagAsync(model,User.GetUserId());
        }


        [HttpGet]
        public async Task<List<PostModel>> GetSubscriptionsFeed([FromQuery]PostRequestModel model)
        {
            return await _postService.GetSubscriptionsFeedAsync(model,User.GetUserId());
        }

        [HttpGet]
        public async Task<List<PostModel>> GetPostsByUserId([FromQuery] PostByUserIdRequestModel model)
        {
            return await _postService.GetUserPostsAsync(model, User.GetUserId());
        }

        [HttpPost]
        public async Task CreatePost(CreatePostModel model)
        {
            var userId = User.GetUserId();
            await _postService.CreatePostAsync(model, userId);
        }

        [Route("{postId}")]
        [HttpPost]
        public async Task DeletePost(Guid postId)
        {
            var userId = User.GetUserId();
            await _postService.DeletePostAsync(postId,userId);
        }

        [HttpPost]
        public async Task UpdatePost(UpdatePostModel model)
        {
            var userId = User.GetUserId();
            await _postService.UpdatePostAsync(model, userId);
        }

        [HttpPost]
        public async Task ChangeLikesVisibility(ChangeLikesVisibilityModel model)
        {
            var userId = User.GetUserId();
            await _postService.ChangeLikesVisibilityAsync(model, userId);
        }

        [HttpPost]
        public async Task ChangeIsCommentsEnabled(ChangeIsCommentsEnabledModel model)
        {
            var userId = User.GetUserId();
            await _postService.ChangeIsCommentsEnabledAsync(model, userId);
        }

        [HttpPost]
        public async Task AddComment(CreateCommentModel model)
        {
            var userId = User.GetUserId();
            await _commentService.AddCommentAsync(model, userId);
        }

        [Route("{commentId}")]
        [HttpPost]
        public async Task DeleteComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _commentService.DeleteCommentAsync(commentId, userId);
        }

        [HttpPost]
        public async Task UpdateComment(UpdateCommentModel model)
        {
            var userId = User.GetUserId();
            await _commentService.UpdateCommentAsync(model, userId);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<CommentModel>> GetCommentsByPost(Guid postId)
        {
            return await _commentService.GetCommentsAsync(postId,User.GetUserId());
        }

        [Route("{postId}")]
        [HttpPost]
        public async Task AddLikePost(Guid postId)
        {
            var userId = User.GetUserId();
            await _likeService.AddLikePostAsync(postId,userId);
        }

        [Route("{postId}")]
        [HttpPost]
        public async Task DeleteLikePost(Guid postId)
        {
            var userId = User.GetUserId();
            await _likeService.DeleteLikePostAsync(postId, userId);
        }

        [Route("{commentId}")]
        [HttpPost]
        public async Task AddLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _likeService.AddLikeCommentAsync(commentId, userId);
        }

        [Route("{commentId}")]
        [HttpPost]
        public async Task DeleteLikeComment(Guid commentId)
        {
            var userId = User.GetUserId();
            await _likeService.DeleteLikeCommentAsync(commentId, userId);
        }

    }
}
