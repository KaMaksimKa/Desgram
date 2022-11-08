namespace Desgram.DAL.Entities
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public User User { get; set; } = null!;
    }
}
