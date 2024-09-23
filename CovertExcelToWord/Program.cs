using Aspose.Cells;
using Aspose.Words;
using Aspose.Words.Tables;
using CovertExcelToWord.Extensions;
using CovertExcelToWord.Models;
using System.Data;
using System.Globalization;

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
        Aspose.Cells.License license = new();
        license.SetLicense("Aspose.Total.655.lic");
        Aspose.Words.License license1 = new();
        license1.SetLicense("Aspose.Total.655.lic");

        string filePath = @"C:\dev\_tmp\裝櫃電子表單0401.xlsm";
        string sheetName = "草稿"; // 替換成您想讀取的工作表名稱

        _wb = new(filePath);
        _ws = _wb.Worksheets[sheetName];

        //Console.WriteLine(ws.Cells.MaxColumn);
        //Console.WriteLine(ws.Cells.MaxRow);
        //Console.WriteLine(ws.Cells.MaxDataColumn);
        //Console.WriteLine(ws.Cells.MaxDataRow);
        //Console.WriteLine(ws.Cells.MaxDisplayRange);

        CabinetInstallationModel cbm = new();
        cbm.HeaderService = GetValue("A2");
        cbm.HeaderCompany = GetValue("J2");
        cbm.HeaderNo = GetValue("A3");
        cbm.ReportTitle = GetValue("A5");
        cbm.DateLocation = GetValue("A7");
        cbm.FooterLeft = $"Kaohsiung, {DateTime.Now.ToString("MMMM d, yyyy", new CultureInfo("en-US"))}";
        cbm.FooterRight = "SGS Far East Ltd., Taiwan";

        CollectionData cdm = new();

        for (int i = 1; i <= _ws.Cells.MaxRow; i++)
        {
            int j = 0; // 用於計算欄位的偏移量
            // 取得 A 欄位的值
            string valueA = GetValue($"A{i}").Trim();
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            switch (valueA)
            {
                case "GOODS":
                    cbm.Goods = GetValue($"E{i}");
                    break;
                case "DESCRIPTION":
                    cbm.Description = GetValue($"E{i}");
                    j = i + 4;
                    if (!string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(GetValue($"E{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(GetValue($"I{j}")))
                                break;
                            cdm.GoodsTable ??= [];
                            cdm.GoodsTable.Add(new GoodsItem()
                            {
                                SONo = GetValue($"E{j}"),
                                InvoiceNo = GetValue($"I{j}"),
                                NetWeight = GetValue($"M{j}"),
                                GrossWeight = GetValue($"Q{j}"),
                                Quantity = GetValue($"U{j}")
                            });
                            j++;
                        }
                        if (cdm.GoodsTable.Any())
                        {
                            GoodsItem lastItem = cdm.GoodsTable.Last();
                            cbm.GoodsTotalNetWeight = lastItem.NetWeight;
                            cbm.GoodsTotalGrossWeight = lastItem.GrossWeight;
                            cbm.GoodsTotalQuantity = lastItem.Quantity;
                            cdm.GoodsTable.Remove(lastItem);
                        }
                    }
                    break;
                case "SHIPMENT":
                    cbm.Shipment = GetValue($"E{i}");
                    break;
                case "SHIPPER":
                    cbm.Shipper = GetValue($"E{i}");
                    break;
                case "BUYER":
                    cbm.Buyer = GetValue($"E{i}");
                    break;
                case "PACKING":
                    cbm.Packing = GetValue($"E{i}");
                    break;
                case "MARKS":
                    cbm.Marks = GetValue($"E{i}");
                    break;
                case "TIME LOG":
                    j = i;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                        string.IsNullOrEmpty(GetValue($"L{j}")))
                            break;
                        //Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"L{j}")}");
                        cdm.TimeLogTable ??= [];
                        cdm.TimeLogTable.Add(new TimeLogItem()
                        {
                            Event = GetValue($"E{j}"),
                            Data = GetValue($"L{j}")
                        });
                        j++;
                    }
                    break;
                case "INSPECTION":
                    cbm.Inspection1 = GetValue($"E{i}");
                    cbm.Inspection2 = GetValue($"E{i + 1}");
                    j = i + 4;
                    if (!string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(GetValue($"I{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(GetValue($"I{j}")))
                                break;
                            cdm.InspectionTable ??= [];
                            cdm.InspectionTable.Add(new InspectionItem()
                            {
                                CoilNo = GetValue($"E{j}"),
                                Findings = GetValue($"I{j}"),
                                PhotoNo = GetValue($"U{j}")
                            });
                            j++;
                        }
                    }
                    break;
                case "QUANTITY":
                    cbm.Quantity = GetValue($"E{i}");
                    j = i + 3;
                    if (!string.IsNullOrEmpty(GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(GetValue($"J{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(GetValue($"I{j}")))
                                break;
                            cdm.QuantityTable ??= [];
                            cdm.QuantityTable.Add(new QuantityItem()
                            {
                                ContainerNo = GetValue($"E{j}"),
                                QuantityLoaded = GetValue($"J{j}"),
                                ShippingSealNo = GetValue($"Q{j}")
                            });
                            j++;
                        }
                    }
                    break;
                case "STOWAGE":
                    cbm.Stowage = GetValue($"E{i}");
                    break;
                case "REMARKS":
                    cbm.Remarks = GetValue($"E{i}");
                    break;
            }


            Console.WriteLine();
            Console.WriteLine(cbm);
        }

        ExportWord(cbm, cdm);
    }

    public static string GetValue(string cellAddress)
    {
        Aspose.Cells.Cell cell = _ws.Cells[cellAddress];
        return cell.DisplayStringValue ?? "";
    }

    //using aspose.word to export word from template with data
    public static void ExportWord(CabinetInstallationModel model, CollectionData cdm)
    {
        // 載入模板文件
        string templatePath = @"Templates\Draft.Container.Loading.docx";
        Document doc = new (templatePath);

        // 準備合併資料
        var data = model.ToDictionary();
        // 執行郵件合併
        doc.MailMerge.Execute(data.Keys.ToArray(), data.Values.ToArray());

        DataTable? tableGoods = cdm.GoodsTable.ToDataTable();
        DataTable? tableTimeLog = cdm.TimeLogTable.ToDataTable();
        DataTable? tableQuantity = cdm.QuantityTable.ToDataTable();
        DataTable? tableInspection = cdm.InspectionTable.ToDataTable();

        DataSet ds = new DataSet();
        ds.Tables.Add(tableGoods);
        ds.Tables.Add(tableTimeLog);
        ds.Tables.Add(tableQuantity);
        ds.Tables.Add(tableInspection);

        doc.MailMerge.ExecuteWithRegions(ds);

        // 移除不需要的行
        //RemoveRowsByBookmark(doc, "GoodsDescription");
        //RemoveRowsByBookmark(doc, "Inspection");
        //RemoveRowsByBookmark(doc, "Quantity");

        // 保存結果文件
        string outputPath = @$"C:\dev\_tmp\{DateTime.Now:yyyyMMddHHmmss}.docx";
        doc.Save(outputPath);

        Console.WriteLine("模板填充完成！");
    }

    /// <summary>
    /// 通過書籤名稱移除表格行
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="bookmarkName"></param>
    public static void RemoveRowsByBookmark(Document doc, string bookmarkName)
    {
        // 獲取指定的書籤
        Bookmark bookmark = doc.Range.Bookmarks[bookmarkName];

        if (bookmark != null)
        {
            // 獲取書籤所在的儲存格
            Aspose.Words.Tables.Cell cell = (Aspose.Words.Tables.Cell)bookmark.BookmarkStart.GetAncestor(NodeType.Cell);

            if (cell != null)
            {
                // 獲取儲存格所在的行
                Aspose.Words.Tables.Row currentRow = cell.ParentRow;

                if (currentRow != null)
                {
                    // 獲取下一行
                    Aspose.Words.Tables.Row nextRow = (Aspose.Words.Tables.Row)currentRow.NextSibling;

                    // 移除當前行
                    currentRow.Remove();

                    // 如果存在下一行，也將其移除
                    if (nextRow != null)
                    {
                        nextRow.Remove();
                    }
                }
            }
        }
    }
}