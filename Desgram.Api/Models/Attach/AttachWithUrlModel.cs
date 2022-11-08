namespace Desgram.Api.Models.Attach
{
    public class AttachWithUrlModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public string MimeType { get; set; } = null!;
    }


}
