using Desgram.DAL.Entities;

namespace Desgram.Api.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int AmountLikes { get; set; } = 0;
    }
}
