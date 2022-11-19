using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Comment
{
    public class CommentRequestModel:SkipTakeModel
    {
        [Required]
        public Guid PostId { get; set; }
    }
}
