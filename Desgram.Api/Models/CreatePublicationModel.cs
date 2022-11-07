using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class CreatePublicationModel
    {
        public string Description { get; set; } = String.Empty;

        [Required]
        public List<MetadataModel> MetadataModels { get; set; } = null!;
    }
}
