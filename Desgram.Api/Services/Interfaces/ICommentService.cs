﻿using Desgram.Api.Models.Comment;

namespace Desgram.Api.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<CommentModel> AddCommentAsync(CreateCommentModel model, Guid requestorId);
        public Task DeleteCommentAsync(Guid commentId, Guid requestorId);
        public Task<List<CommentModel>> GetCommentsAsync(CommentRequestModel model, Guid requestorId);
        public Task UpdateCommentAsync(UpdateCommentModel model, Guid requestorId);
        public Task DeleteAllCommentsFromUserAsync(Guid userId,Guid requestorId);
    }
}
