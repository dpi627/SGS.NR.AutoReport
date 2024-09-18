using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;
using MiniExcelLibs;

namespace CovertExcelToWord
{
    internal class Program
    {

        private static List<dynamic>? _rows;
        private static readonly List<string> _subtitles = [
            "GOODS","DESCRIPTION","SHIPMENT","SHIPPER","BUYER","PACKING","MARKS","TIME LOG","INSPECTION","QUANTITY","STOWAGE","REMARKS"
            ];

        static void Main(string[] args)
        {
            string filePath = @"C:\dev\_tmp\裝櫃電子表單0401.xlsm";
            string sheetName = "草稿"; // 替換成您想讀取的工作表名稱

            // 使用 MiniExcel 讀取所有資料到列表
            _rows = MiniExcel.Query(filePath, sheetName: sheetName).ToList();

            try
            {
                for (int i = 1; i <= _rows.Count; i++)
                {
                    Console.WriteLine(i);
                    string cellValue = GetValue($"A{i}");

                    if (string.IsNullOrWhiteSpace(cellValue))
                        continue;

                    if (_subtitles.Contains(cellValue.Trim()))
                    {
                        Console.WriteLine(cellValue);
                        Console.WriteLine(GetValue($"E{i}"));

                        // INSPECTION 較特殊，要額外多抓一行
                        if (cellValue == "INSPECTION")
                        {
                            Console.WriteLine(GetValue($"E{i + 1}"));
                        }

                        // 每種表格處理方式不同，只能分開寫
                        if (cellValue == "DESCRIPTION" &&
                            !string.IsNullOrEmpty(GetValue($"E{i + 3}")) &&
                            !string.IsNullOrEmpty(GetValue($"I{i + 3}")))
                        {
                            int j = i + 3;
                            while (true)
                            {
                                Console.WriteLine(j);
                                if (string.IsNullOrEmpty(GetValue($"E{j}")))
                                {
                                    break;
                                }
                                Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"I{j}")} {GetValue($"M{j}")} {GetValue($"Q{j}")} {GetValue($"U{j}")}");
                                j++;
                            }
                            //i = j;
                        }

                        if (cellValue == "INSPECTION" &&
                            !string.IsNullOrEmpty(GetValue($"E{i + 3}")) &&
                            !string.IsNullOrEmpty(GetValue($"I{i + 3}")))
                        {
                            int j = i + 3;
                            while (true)
                            {
                                Console.WriteLine(j);
                                if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                                    string.IsNullOrEmpty(GetValue($"I{j}")))
                                {
                                    break;
                                }
                                Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"I{j}")} {GetValue($"U{j}")}");
                                j++;
                            }
                            //i = j;
                        }

                        if (cellValue == "QUANTITY" &&
                            !string.IsNullOrEmpty(GetValue($"E{i + 2}")) &&
                            !string.IsNullOrEmpty(GetValue($"J{i + 2}")))
                        {
                            int j = i + 2;
                            while (true)
                            {
                                Console.WriteLine(j);
                                if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                                    string.IsNullOrEmpty(GetValue($"J{j}")))
                                {
                                    break;
                                }
                                Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"J{j}")} {GetValue($"Q{j}")}");
                                j++;
                            }
                            //i = j;
                        }
                    }
                }


                //while (true)
                //{
                //    // 提示使用者輸入儲存格位置
                //    Console.Write("請輸入儲存格位置（例如 C10）：");
                //    string cellAddress = Console.ReadLine();

                //    // 讀取指定儲存格的值
                //    string cellValue = GetValue(cellAddress);

                //    Console.WriteLine($"儲存格 {cellAddress} 的值為： {cellValue}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤： {ex.Message}");
            }
        }

        private static string GetValue(string CellAddress)
        {
            string cellAddress = CellAddress.ToUpper();

            // 解析儲存格位置
            (int columnNumber, int rowNumber) = ParseCellAddress(cellAddress);

            // 讀取指定儲存格的值
            string cellValue = GetCellValue(_rows, columnNumber, rowNumber);

            return cellValue;
        }

        /// <summary>
        /// 解析儲存格地址，例如 "C10" 解析為 (3, 10)
        /// </summary>
        /// <param name="cellAddress">儲存格地址</param>
        /// <returns>列號和行號</returns>
        static (int column, int row) ParseCellAddress(string cellAddress)
        {
            if (string.IsNullOrWhiteSpace(cellAddress))
                throw new ArgumentException("儲存格地址不能為空。");

            int splitIndex = 0;
            for (; splitIndex < cellAddress.Length; splitIndex++)
            {
                if (char.IsDigit(cellAddress[splitIndex]))
                    break;
            }

            if (splitIndex == 0 || splitIndex == cellAddress.Length)
                throw new ArgumentException("無效的儲存格地址格式。");

            string columnPart = cellAddress.Substring(0, splitIndex);
            string rowPart = cellAddress.Substring(splitIndex);

            if (!int.TryParse(rowPart, out int rowNumber))
                throw new ArgumentException("無效的行號。");

            int columnNumber = ColumnLetterToNumber(columnPart);

            return (columnNumber, rowNumber);
        }

        /// <summary>
        /// 將列字母轉換為數字，例如 "A" -> 1, "B" -> 2, ..., "AA" -> 27
        /// </summary>
        /// <param name="column">列字母</param>
        /// <returns>列數字</returns>
        static int ColumnLetterToNumber(string column)
        {
            if (string.IsNullOrEmpty(column))
                throw new ArgumentException("列名稱不能為空。");

            int sum = 0;
            foreach (char c in column)
            {
                if (c < 'A' || c > 'Z')
                    throw new ArgumentException("欄位名稱必須為大寫字母。");

                sum *= 26;
                sum += (c - 'A' + 1);
            }
            return sum;
        }

        /// <summary>
        /// 根據列號和行號讀取儲存格的值
        /// </summary>
        /// <param name="filePath">Excel 檔案路徑</param>
        /// <param name="sheetName">工作表名稱</param>
        /// <param name="columnNumber">列號</param>
        /// <param name="rowNumber">行號</param>
        /// <returns>儲存格的值</returns>
        static string GetCellValue(List<dynamic> rows, int columnNumber, int rowNumber)
        {
            // 使用 MiniExcel 讀取所有資料到列表
            //var _rows = MiniExcel.Query(filePath, sheetName: sheetName).ToList();

            // 檢查行號是否在範圍內
            if (rowNumber < 1 || rowNumber > rows.Count)
                throw new IndexOutOfRangeException("行號超出範圍。");

            // 取得目標行
            IDictionary<string, object>? targetRow = rows[rowNumber - 1] as IDictionary<string, object> ??
                throw new Exception("無法讀取該行資料。");

            // 取得所有欄位名稱並排序
            var headers = targetRow.Keys.ToList();
            headers.Sort((a, b) => ColumnLetterToNumber(a) - ColumnLetterToNumber(b));

            if (columnNumber < 1 || columnNumber > headers.Count)
                throw new IndexOutOfRangeException("欄位超出範圍。");

            string targetHeader = headers[columnNumber - 1];

            // 取得儲存格值
            object cellValue = targetRow.ContainsKey(targetHeader) ? targetRow[targetHeader] : null;

            return cellValue?.ToString() ?? string.Empty;
        }

    }
}
