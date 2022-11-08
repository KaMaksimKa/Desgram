namespace Desgram.DAL.Entities
{
    public class AttachPublication:Attach
    {
        public Guid PublicationId { get; set; }
        public virtual Publication Publication { get; set; } = null!;
    }
}
