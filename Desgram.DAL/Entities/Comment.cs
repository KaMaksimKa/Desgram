namespace Desgram.DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PublicationId { get; set; }
        public int AmountLikes { get; set; } = 0;
        public DateTimeOffset CreatedDate { get; set; }

        public User? User { get; set; }
        public Publication? Publication { get; set; }
        public List<LikeComment> LikesComment { get; set; } = null!;
    }
}
