using System.ComponentModel.DataAnnotations;
using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Comment
{
    public class UpdateCommentModel
    {
        [Required]
        public Guid CommentId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

    }
}
