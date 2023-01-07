namespace Desgram.Api.Models.Push
{
    public interface IPushModel
    {
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; }
        public Dictionary<string, object>? CustomData { get; set; }
    }
}
