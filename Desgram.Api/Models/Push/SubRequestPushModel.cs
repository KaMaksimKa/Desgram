namespace Desgram.Api.Models.Push
{
    public class SubRequestPushModel:IPushModel
    {
        public SubRequestPushModel(string userName)
        {
            Alert = new AlertModel()
            {

                Body = $"{userName} хочет подписаться на вас.",
            };
        }
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; }
        public Dictionary<string, object>? CustomData { get; set; }
    }


}
