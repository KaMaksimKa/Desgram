namespace Desgram.Api.Models
{
    public class PublicationContentModel
    {
        public Guid Id { get; set; }
        public string AttachPath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
    }
}
