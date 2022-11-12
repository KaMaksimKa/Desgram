namespace Desgram.DAL.Entities
{
    public class AttachPost:Attach
    {
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
