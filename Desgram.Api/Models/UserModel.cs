namespace Desgram.Api.Models
{
    public class UserModel
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public int AmountSubscriptions { get; set; }
        public int AmountSubscribers { get; set; }

    }
}
