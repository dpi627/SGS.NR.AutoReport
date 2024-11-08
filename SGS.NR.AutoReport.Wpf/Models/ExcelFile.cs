#nullable disable

namespace SGS.NR.AutoReport.Wpf.Models;

public record ExcelFile
{
    public string FileName { get; init; }
    public string FilePath { get; init; }
}
