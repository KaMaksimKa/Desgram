namespace Desgram.Api.Models.Push
{
    public class PushModel:IPushModel
    {
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; } = null!;
        public Dictionary<string,object>? CustomData { get; set; }
      
    }
    
}
