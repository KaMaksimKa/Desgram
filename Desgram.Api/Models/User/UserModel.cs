using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = null!;
        public AttachWithUrlModel? Avatar { get; set; }
        public int AmountFollowing { get; set; }
        public int AmountFollowers { get; set; }
        public int AmountPosts { get; set; }
        public bool IsPrivate { get; set; }
        
    }
}
