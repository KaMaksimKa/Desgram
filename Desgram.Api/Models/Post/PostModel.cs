using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public PartialUserModel User { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? AmountLikes { get; set; }
        public int? AmountComments { get; set; }
        public bool IsCommentsEnabled { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<ImageContentModel> ImageContents { get; set; } = null!;
        public bool HasLiked { get; set; }
    }
}
