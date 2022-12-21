using System.ComponentModel.DataAnnotations;
using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Comment
{
    public class CreateCommentModel
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        
    }
}
