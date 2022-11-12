using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Comment
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int AmountLikes { get; set; } = 0;
        public bool IsEdit { get; set; }
    }
}
