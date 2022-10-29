namespace Desgram.DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PublicationId { get; set; }
        public Publication Publication { get; set; }
        public List<LikeComment> LikesComment { get; set; } = new List<LikeComment>();
        public int AmountLikes { get; set; } = 0;
    }
}
