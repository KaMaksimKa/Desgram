using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class UpdatePostModel
    {
        [Required]
        public Guid PostId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
