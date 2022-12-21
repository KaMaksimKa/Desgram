using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class EmailCodeModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required] 
        public string Code { get; set; } = null!;
    }
}
