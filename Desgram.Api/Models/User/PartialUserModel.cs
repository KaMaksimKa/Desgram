using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.User
{
    public class PartialUserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ImageWithUrlModel? Avatar { get; set; }
    
    }
}
