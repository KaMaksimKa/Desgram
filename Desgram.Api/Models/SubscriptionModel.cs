using System.ComponentModel.DataAnnotations;

namespace Desgram.Api.Models
{
    public class SubscriptionModel
    {
        [Required]
        public string UserName { get; set; } = null!;
    }
}
