﻿namespace Desgram.Api.Models.Push
{
    public class PushModel
    {
        public int? Badge { get; set; }
        public string? Sound { get; set; }
        public AlertModel Alert { get; set; } = null!;
        public Dictionary<string,object>? CustomData { get; set; }
        public class AlertModel
        {
            public string? Title { get; set; }
            public string? Subtitle { get; set; }
            public string? Body { get; set; }
        }
    }
}
