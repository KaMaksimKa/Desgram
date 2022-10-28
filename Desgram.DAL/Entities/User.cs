namespace Desgram.DAL.Entities
{
    public class User
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
    }
}
