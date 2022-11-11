using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class ChangeUserNameModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._]*")]
        public string NewName { get; init; } = null!;
    }
}
