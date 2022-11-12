namespace Desgram.DAL.Entities
{
    public class LikePost:Like
    {
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
