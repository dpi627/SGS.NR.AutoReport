#nullable disable

namespace SGS.NR.Repository.DTO.Condition;

public record VesselLoadingCondition
{
    public string SourcePath { get; set; }
    public string SheetName { get; set; } = "草稿";
}
