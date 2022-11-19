namespace Desgram.DAL.Entities
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = null!;

    }
}
