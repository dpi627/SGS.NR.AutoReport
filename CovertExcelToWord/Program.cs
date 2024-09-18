using MiniExcelLibs;
using MiniSoftware;

namespace CovertExcelToWord;

internal class Program
{
    static void Main(string[] args)
    {
        string filePath = @"C:\dev\_tmp\裝櫃電子表單0401.xlsm";
        string sheetName = "草稿"; // 替換成您想讀取的工作表名稱

        var rows = MiniExcel.Query(filePath, sheetName: sheetName).ToList();

        foreach (var row in rows)
        {
            var rowDict = (IDictionary<string, object>)row;
            for (char col = 'A'; col <= 'X'; col++)
            {
                string cellValue = rowDict[col.ToString()]?.ToString();
                Console.Write($"{cellValue}\t");
            }
            Console.WriteLine(); // 換行,開始下一行
        }
    }
}
