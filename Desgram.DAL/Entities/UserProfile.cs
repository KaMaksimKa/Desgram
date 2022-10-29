namespace Desgram.DAL.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Biography { get; set; }
        public ImageUserProfile ImageUserProfile { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset? BirthDate { get; init; }
    }

}
