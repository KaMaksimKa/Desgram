using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Push
{
    public class LikeCommentPushModel:IPushModel
    {
        public LikeCommentPushModel(string userName,string comment)
        {
            Alert = new AlertModel()
            {

                Body = $"{userName} нравится ваш комментарий: {comment}",
            };
        }
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; }
        public Dictionary<string, object>? CustomData { get; set; }
    }
}
