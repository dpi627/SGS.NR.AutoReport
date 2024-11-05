#nullable disable

namespace SGS.NR.Service.DTO.Info;

public record ContainerLoadingInfo
{
    public string SourcePath { get; set; }
    public string SheetName { get; set; } = "草稿";
    public string TemplatePath { get; set; }
    public string TargetPath { get; set; }
}
