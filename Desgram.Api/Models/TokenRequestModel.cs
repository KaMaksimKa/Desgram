namespace Desgram.Api.Models
{
    public class TokenRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public TokenRequestModel(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
