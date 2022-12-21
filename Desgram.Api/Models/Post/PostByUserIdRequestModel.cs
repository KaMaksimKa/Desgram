using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostByUserIdRequestModel:SkipTakeModel
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
