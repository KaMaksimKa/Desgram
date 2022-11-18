using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class SearchUsersByNameModel:SkipTakeModel
    {
        [Required]
        public string SearchUserName { get; set; } = null!;
    }
}
