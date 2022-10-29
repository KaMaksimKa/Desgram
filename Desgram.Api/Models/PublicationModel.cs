using Desgram.DAL.Entities;

namespace Desgram.Api.Models
{
    public class PublicationModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = String.Empty;
        public List<Guid> ImageGuidList { get; set; }
        public int AmountLikes { get; set; }
    }
}
