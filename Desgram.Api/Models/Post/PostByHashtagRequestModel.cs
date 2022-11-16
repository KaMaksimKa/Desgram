using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models.Post
{
    public class PostByHashtagRequestModel:PostRequestModel
    {
        [Required] 
        public string Hashtag { get; set; } = null!;
    }
}
