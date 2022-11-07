namespace Desgram.DAL.Entities
{
    public class Publication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = null!;
        public int AmountLikes { get; set; } = 0;
        public int AmountComments { get; set; } = 0;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }


        public virtual User? User { get; set; }
        public virtual List<AttachPublication>? AttachPublications { get; set; }
        public virtual List<LikePublication>? LikesPublication { get; set; }
        public virtual List<Comment>? Comments { get; set; }
        public virtual List<HashTag>? HashTags { get; set; }
    }
}
