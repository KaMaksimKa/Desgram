using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.User
{
    public class UserModel
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public int AmountSubscriptions { get; set; }
        public int AmountSubscribers { get; set; }
        public AttachWithUrlModel? Avatar { get; set; }
    }
}
