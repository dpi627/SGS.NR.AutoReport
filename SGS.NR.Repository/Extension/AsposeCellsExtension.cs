namespace SGS.NR.Repository.Extension;

public static class AsposeCellsExtension
{
    public static string GetValue(this Aspose.Cells.Worksheet ws, string cellAddress)
    {
        Aspose.Cells.Cell cell = ws.Cells[cellAddress];
        var data = cell.DisplayStringValue ?? "";
        return string.IsNullOrEmpty(data) ? "" : FixFormat(data);
    }

    private static string FixFormat(string source)
    {
        return source.Replace("’", "'");
    }
}
