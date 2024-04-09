namespace SGS.NR.Service.DTO.Info
{
    public record DraftInfo
    {
        public string? ExportPath { get; set; }
        public string? TemplatePath { get; set; }
        public IDictionary<string, object>? Data { get; set; }
    }
}
