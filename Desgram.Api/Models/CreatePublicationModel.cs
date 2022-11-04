namespace Desgram.Api.Models
{
    public class CreatePublicationModel
    {
        public string Description { get; set; } = String.Empty;
         
        public List<MetadataModel> MetadataModels { get; set; } = new List<MetadataModel>();
    }
}
