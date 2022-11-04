namespace Desgram.Api.Models
{
    public class PublicationModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Guid> ImageGuidList { get; set; } = new List<Guid>();
        public int AmountLikes { get; set; }
        public int AmountComments { get; set; }
    }
}
