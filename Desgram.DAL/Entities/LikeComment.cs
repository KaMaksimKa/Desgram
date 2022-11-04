namespace Desgram.DAL.Entities
{
    public class LikeComment:Like
    {
        public Guid CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
