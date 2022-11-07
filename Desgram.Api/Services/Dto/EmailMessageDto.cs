namespace Desgram.Api.Services.Dto
{
    public class EmailMessageDto
    {
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
