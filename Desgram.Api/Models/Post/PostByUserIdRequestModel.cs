using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostByUserIdRequestModel:PostRequestModel
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
