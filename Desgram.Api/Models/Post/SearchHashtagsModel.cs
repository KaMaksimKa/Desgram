using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class SearchHashtagsModel:SkipTakeModel
    {
        [Required]
        public string SearchString { get; set; } = null!;

    }
}
