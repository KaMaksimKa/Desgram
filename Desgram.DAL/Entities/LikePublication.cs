namespace Desgram.DAL.Entities
{
    public class LikePublication:Like
    {
        public Guid PublicationId { get; set; }
        public Publication? Publication { get; set; }
    }
}
