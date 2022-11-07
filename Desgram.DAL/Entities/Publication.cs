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

        public User? User { get; set; }
        public List<AttachPublication>? AttachPublications { get; set; }
        public List<LikePublication>? LikesPublication { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<HashTag>? HashTags { get; set; }
    }
}
