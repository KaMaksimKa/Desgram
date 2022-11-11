using System.ComponentModel.DataAnnotations;
using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Publication
{
    public class UpdateCommentModel
    {
        [Required]
        public string Content { get; set; } = null!;

    }
}
