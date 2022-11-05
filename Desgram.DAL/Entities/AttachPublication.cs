namespace Desgram.DAL.Entities
{
    public class AttachPublication:Attach
    {
        public Guid PublicationId { get; set; }
        public Publication? Publication { get; set; }
    }
}
