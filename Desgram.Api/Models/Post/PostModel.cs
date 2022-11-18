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
        public List<AttachWithUrlModel> AttachesPost { get; set; } = null!;
        public List<string> HashTags { get; set; } = null!;
        public bool IsLiked { get; set; }
        public bool IsAuthor { get; set; }
    }
}
