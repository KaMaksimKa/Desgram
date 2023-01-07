namespace Desgram.Api.Models.Push
{
    public class LikePostPushModel:IPushModel
    {

        public LikePostPushModel(string userName)
        {
            Alert = new AlertModel()
            {

                Body = $"{userName} нравится ваша публикация",
            };
        }
        public int? Badge { get; set; }
        public string? Sound { get; set; }

        public AlertModel Alert { get; set; } 
        public Dictionary<string, object>? CustomData { get; set; }
    }
}
