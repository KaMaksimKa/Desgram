using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class DeleteCommentModel
    {
        [Required]
        public Guid CommentId { get; set; }
    }
}
