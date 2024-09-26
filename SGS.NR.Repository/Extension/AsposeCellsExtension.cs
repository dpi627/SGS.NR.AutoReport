namespace SGS.NR.Repository.Extension;

public static class AsposeCellsExtension
{
    public static string GetValue(this Aspose.Cells.Worksheet ws, string cellAddress)
    {
        Aspose.Cells.Cell cell = ws.Cells[cellAddress];
        return cell.DisplayStringValue ?? "";
    }
}
