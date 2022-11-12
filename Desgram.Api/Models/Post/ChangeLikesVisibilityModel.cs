using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class ChangeLikesVisibilityModel
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public bool IsLikesVisible { get; set; }
    }
}
