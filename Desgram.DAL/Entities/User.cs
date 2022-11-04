namespace Desgram.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Biography { get; set; }
        public DateTimeOffset? BirthDate { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
        public Avatar? Avatar { get; set; }

    }
}
