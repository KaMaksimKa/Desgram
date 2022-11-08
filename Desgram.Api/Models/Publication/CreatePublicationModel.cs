using System.ComponentModel.DataAnnotations;
using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.Publication
{
    public class CreatePublicationModel
    {
        public string Description { get; set; } = string.Empty;

        [Required]
        public List<MetadataModel> MetadataModels { get; set; } = null!;
    }
}
