namespace SGS.NR.Service.DTO.Info
{
    public record DraftInfo
    {
        public string? ImportPath { get; set; }
        public string? ExportPath { get; set; }
        public string? TemplatePath { get; set; }
    }
}
