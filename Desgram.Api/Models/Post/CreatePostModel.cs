using System.ComponentModel.DataAnnotations;
using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.Post
{
    public class CreatePostModel
    {
        public string Description { get; set; } = string.Empty;

        [Required]
        public List<MetadataModel> MetadataModels { get; set; } = null!;

        public bool IsCommentsEnabled { get; set; } = true;

        public bool IsLikesVisible { get; set; } = true;
    }
}
