namespace Desgram.Api.Config
{
    public class AdminUserConfig
    {
        public const string Position = "AdminUser";
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
    }
}
