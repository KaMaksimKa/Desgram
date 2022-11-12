using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class ChangeIsCommentsEnabledModel
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public bool IsCommentsEnabled { get; set; }
    }
}
