namespace Desgram.DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PublicationId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }


        public virtual User User { get; set; } = null!;
        public virtual Publication Publication { get; set; } = null!;
        public virtual ICollection<LikeComment> LikesComment { get; set; } = null!;
    }
}
