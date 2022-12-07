namespace Desgram.Api.Models.User
{
    public class PersonalInformationModel
    {
        public string Email { get; set; } = null!;
        public DateTimeOffset? BirthDate { get; set; }
    }
}
