using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.User
{
    public class EmailCodeRequestModel
    {
        [Required] 
        public string Email { get; set; } = null!;


    }
}
