using Desgram.Api.Models.Attach;
using Desgram.Api.Models.User;

namespace Desgram.Api.Models.Post
{
    public class PartialPostModel
    {
        public Guid Id { get; set; }
        public ImageWithUrlModel PreviewImage { get; set; } = null!;

    }
}
