namespace Desgram.DAL.Entities
{
    public class ImagePublication:Attach
    {
        public Guid PublicationId { get; set; }
        public Publication? Publication { get; set; }
    }
}
