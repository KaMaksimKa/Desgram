namespace Desgram.Api.Models.Push
{
    public class SubscriptionPushModel:IPushModel
    {
        public SubscriptionPushModel(string userName)
        {
            Alert = new AlertModel()
            {

                Body = $"{userName} подписался(-ась) на ваши обновления.",
            };
        }
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; }
        public Dictionary<string, object>? CustomData { get; set; }
    }

}
