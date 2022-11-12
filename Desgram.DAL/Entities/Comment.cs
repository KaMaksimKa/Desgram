namespace Desgram.DAL.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }


        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<LikeComment> Likes { get; set; } = null!;
    }
}
