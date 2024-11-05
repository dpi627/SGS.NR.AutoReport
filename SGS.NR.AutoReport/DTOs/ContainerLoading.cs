#nullable disable

namespace SGS.NR.AutoReport.DTOs;

public record ContainerLoading
{
    public string SourcePath { get; init; }
    public string TemplatePath { get; init; }
    public string TargetPath { get; init; }
}
