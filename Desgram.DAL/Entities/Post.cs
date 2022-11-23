namespace Desgram.DAL.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool IsCommentsEnabled { get; set; }
        public bool IsLikesVisible { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<PostImageContent> ImagePostContents { get; set; } = null!;
        public virtual ICollection<LikePost> Likes { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
        public virtual ICollection<HashTag> HashTags { get; set; } = null!;
    }
}
