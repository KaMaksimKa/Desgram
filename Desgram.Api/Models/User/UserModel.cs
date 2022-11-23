using Desgram.Api.Models.Attach;

namespace Desgram.Api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; init; } = null!;
        public string FullName { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public ImageContentModel? Avatar { get; set; }
        public int AmountFollowing { get; set; }
        public int AmountFollowers { get; set; }
        public int AmountPosts { get; set; }
        public bool IsPrivate { get; set; }
        public bool FollowedByViewer { get; set; }
        public bool HasRequestedViewer { get; set; }
        public bool FollowsViewer { get; set; }
        public bool HasBlockedViewer { get; set; }



    }
}
