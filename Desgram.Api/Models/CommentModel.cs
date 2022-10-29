using Desgram.DAL.Entities;

namespace Desgram.Api.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public int AmountLikes { get; set; } = 0;
    }
}
