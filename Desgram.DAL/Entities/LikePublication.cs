namespace Desgram.DAL.Entities
{
    public class LikePublication:Like
    {
        public Guid PublicationId { get; set; }
        public virtual Publication Publication { get; set; } = null!;
    }
}
