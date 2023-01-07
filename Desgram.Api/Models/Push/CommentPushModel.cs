using Desgram.DAL.Entities;

namespace Desgram.Api.Models.Push
{
    public class CommentPushModel:IPushModel
    {
        public CommentPushModel(string userName, string content)
        {
            Alert = new AlertModel()
            {

                Body = $"{userName} прокомментировал(-а): {content}",
            };
        }
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; }
        public Dictionary<string, object>? CustomData { get; set; }
    }
}

