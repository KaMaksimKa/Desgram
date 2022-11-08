namespace Desgram.DAL.Entities
{
    public class Publication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }


        public virtual User User { get; set; } = null!;
        public virtual ICollection<AttachPublication> AttachesPublication { get; set; } = null!;
        public virtual ICollection<LikePublication> LikesPublication { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
        public virtual ICollection<HashTag> HashTags { get; set; } = null!;
    }
}
