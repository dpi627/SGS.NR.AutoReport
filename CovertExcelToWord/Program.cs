using Aspose.Cells;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace CovertExcelToWord;

public class Program
{
    private static Workbook _wb;
    private static Worksheet _ws;
    private static readonly List<string> _subtitles = [
        "GOODS","DESCRIPTION","SHIPMENT","SHIPPER","BUYER","PACKING","MARKS","TIME LOG","INSPECTION","QUANTITY","STOWAGE","REMARKS"
        ];

    static void Main()
    {
        License license = new();
        license.SetLicense("Aspose.Total.655.lic");

        string filePath = @"C:\dev\_tmp\裝櫃電子表單0401.xlsm";
        string sheetName = "草稿"; // 替換成您想讀取的工作表名稱

        _wb = new(filePath);
        _ws = _wb.Worksheets[sheetName];

        //Console.WriteLine(ws.Cells.MaxColumn);
        //Console.WriteLine(ws.Cells.MaxRow);
        //Console.WriteLine(ws.Cells.MaxDataColumn);
        //Console.WriteLine(ws.Cells.MaxDataRow);
        //Console.WriteLine(ws.Cells.MaxDisplayRange);

        for(int i=0; i<_ws.Cells.MaxRow; i++)
        {
            string valueA = GetValue($"A{i + 1}").Trim();

            if(string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            Console.WriteLine(GetValue($"A{i + 1}").Trim());

            if (valueA == "TIME LOG")
            {
                int j = i + 1;
                while (true)
                {
                    if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    string.IsNullOrEmpty(GetValue($"L{j}")))
                        break;
                    Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"L{j}")}");
                    j++;
                }
            }
            else
                Console.WriteLine(GetValue($"E{i + 1}"));

            if (valueA == "INSPECTION")
                Console.WriteLine(GetValue($"E{i + 2}"));

            if (valueA == "DESCRIPTION" && (
                !string.IsNullOrEmpty(GetValue($"E{i + 4}")) &&
                !string.IsNullOrEmpty(GetValue($"E{i + 4}"))
                ))
            {
                int j = i + 4;
                while (true)
                {
                    if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    string.IsNullOrEmpty(GetValue($"I{j}")))
                        break;
                    Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"I{j}")} {GetValue($"M{j}")} {GetValue($"Q{j}")} {GetValue($"U{j}")} {GetValue($"W{j}")}");
                    j++;
                }
                //i = j;
            }

            if (valueA == "INSPECTION" && (
                !string.IsNullOrEmpty(GetValue($"E{i + 4}")) &&
                !string.IsNullOrEmpty(GetValue($"I{i + 4}"))
                ))
            {
                int j = i + 4;
                while (true)
                {
                    if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    string.IsNullOrEmpty(GetValue($"I{j}")))
                        break;
                    Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"I{j}")} {GetValue($"U{j}")}");
                    j++;
                }
                //i = j;
            }

            if (valueA == "QUANTITY" && (
                !string.IsNullOrEmpty(GetValue($"E{i + 3}")) &&
                !string.IsNullOrEmpty(GetValue($"J{i + 3}"))
                ))
            {
                int j = i + 3;
                while (true)
                {
                    if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    string.IsNullOrEmpty(GetValue($"I{j}")))
                        break;
                    Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"J{j}")} {GetValue($"Q{j}")}");
                    j++;
                }
                //i = j;
            }


            Console.WriteLine();
        }
    }

    public static string GetValue(string cellAddress)
    {
        Cell cell = _ws.Cells[cellAddress];
        return cell.DisplayStringValue ?? "";
        //object? value = _ws.Cells[cellAddress].Value;
        //return value == null ? "" : value.ToString() ?? "";
    }
}