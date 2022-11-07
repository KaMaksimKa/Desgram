namespace Desgram.Api.Models
{
    public class UserModel
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public int AmountSubscriptions { get; set; }
        public int AmountSubscribers { get; set; }

    }
}
