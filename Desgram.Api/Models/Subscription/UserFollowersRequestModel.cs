namespace Desgram.Api.Models.Subscription
{
    public class UserFollowersRequestModel:SkipTakeModel
    {
        public Guid UserId { get; set; }
    }
}
