using Desgram.Api.Models.User;
using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Comment
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public PartialUserModel User { get; set; } = null!;
        public int AmountLikes { get; set; } = 0;
        public bool HasEdit { get; set; }
        public bool HasLiked { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
