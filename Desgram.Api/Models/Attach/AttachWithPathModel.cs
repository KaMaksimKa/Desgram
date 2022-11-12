namespace Desgram.Api.Models.Attach
{
    public class AttachWithPathModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
