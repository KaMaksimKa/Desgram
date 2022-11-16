using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class SkipTakeModel
    {
        [Required]
        public int Skip { get; set; }

        [Required]
        public int Take { get; set; }
    }
}
