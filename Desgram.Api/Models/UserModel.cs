namespace Desgram.Api.Models
{
    public class UserModel
    {
        public string Name { get; init; }
        public string Email { get; init; }

        public UserModel(string name, string email, DateTimeOffset birthDate)
        {
            Name = name;
            Email = email;
        }
    }
}
