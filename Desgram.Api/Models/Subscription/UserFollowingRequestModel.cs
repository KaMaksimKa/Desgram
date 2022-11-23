namespace Desgram.Api.Models.Subscription
{
    public class UserFollowingRequestModel:SkipTakeModel
    {
        public Guid UserId { get; set; }
    }
}
