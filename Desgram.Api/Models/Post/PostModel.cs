using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? AmountLikes { get; set; }
        public int? AmountComments { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<AttachWithUrlModel> AttachesPost { get; set; } = null!;
        public List<string> HashTags { get; set; } = null!;
        public bool IsLiked { get; set; }
    }
}
