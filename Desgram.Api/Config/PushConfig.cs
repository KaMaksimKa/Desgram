namespace Desgram.Api.Config
{
    public class PushConfig
    {
        public const string Position = nameof(PushConfig);
        public GoogleConfig Google { get; set; } = null!;

        public class GoogleConfig
        {
            public string? ServerKey { get; set; }
            public string? SenderId { get; set; }
            public string? GcmUrl { get; set; }
        }
    }
}
