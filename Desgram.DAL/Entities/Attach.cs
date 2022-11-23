namespace Desgram.DAL.Entities
{
    public class Attach
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string Path { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public Guid OwnerId { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }


        public virtual User Owner { get; set; } = null!;
    }
}
