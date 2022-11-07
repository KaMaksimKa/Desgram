using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Desgram.Api.Config
{
    public class EmailConfig
    {
        public const string Position = "email";
        public string DesgramEmailAddress { get; set; } = null!;
        public string DesgramEmailAddressPassword { get; set; } = null!;
        public string ConnectAddress { get; set; } = null!;
        public int ConnectPort { get; set; }
        public bool UseSsl { get; set; }
        
    }
}
