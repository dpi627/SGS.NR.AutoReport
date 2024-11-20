using Aspose.Cells;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.ContainerLoading;
using SGS.NR.Repository.Interface;
using System.Globalization;
using SGS.NR.Repository.Extension;

namespace SGS.NR.Repository.Implement;

public class ContainerLoadingRepository : BaseRepository, IContainerLoadingRepository
{
    private Workbook _wb;
    private Worksheet _ws;
    private readonly List<string> _subtitles = [
        "GOODS","DESCRIPTION","SHIPMENT","SHIPPER","BUYER","PACKING","MARKS",
        "TIME LOG","INSPECTION","QUANTITY","STOWAGE","REMARKS"
        ];

    public MainDataModel Read(ContainerLoadingCondition condition)
    {
        var model = new MainDataModel();
        // 取得 Excel
        _wb = new(condition.SourcePath);
        // 取得工作表
        _ws = _wb.Worksheets[condition.SheetName];
        // 設定一次性或固定資料
        SetSingleData(model.SingleData);
        // 設定集合(迴圈)資料
        SetCollectionData(model.CollectionData);
        // 其他資料處理
        OtherDataProcessing(model);

        return model;
    }

    /// <summary>
    /// 設定部分一次性資料、固定位置資料
    /// </summary>
    private void SetSingleData(SingleDataModel data)
    {
        // 固定位置或內容之資料
        data.HeaderService = _ws.GetValue("A2");
        data.HeaderCompany = _ws.GetValue("J2");
        data.HeaderNo = _ws.GetValue("A3");
        data.ReportTitle = _ws.GetValue("A5");
        data.DateLocation = _ws.GetValue("A7");
        data.FooterLeft = $"Kaohsiung, {DateTime.Now.ToString("MMMM d, yyyy", new CultureInfo("en-US"))}";
        data.FooterRight = "SGS Far East Ltd., Taiwan";

        //不固定位置之資料，只能在迴圈期間抓取
        string valueA; //A欄資料
        for (int i = 1; i <= _ws.Cells.MaxRow; i++)
        {
            valueA = _ws.GetValue($"A{i}").Trim();
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            switch (valueA)
            {
                case "GOODS":
                    data.Goods = _ws.GetValue($"E{i}");
                    break;
                case "DESCRIPTION":
                    data.Description = _ws.GetValue($"E{i}");
                    break;
                case "SHIPMENT":
                    data.Shipment = _ws.GetValue($"E{i}");
                    break;
                case "SHIPPER":
                    data.Shipper = _ws.GetValue($"E{i}");
                    break;
                case "BUYER":
                    data.Buyer = _ws.GetValue($"E{i}");
                    break;
                case "PACKING":
                    data.Packing = _ws.GetValue($"E{i}");
                    break;
                case "MARKS":
                    data.Marks = _ws.GetValue($"E{i}");
                    break;
                case "INSPECTION":
                    data.Inspection1 = _ws.GetValue($"E{i}");
                    data.Inspection2 = _ws.GetValue($"E{i + 1}");
                    break;
                case "QUANTITY":
                    data.Quantity = _ws.GetValue($"E{i}");
                    break;
                case "STOWAGE":
                    data.Stowage = _ws.GetValue($"E{i}");
                    break;
                case "REMARKS":
                    data.Remarks = _ws.GetValue($"E{i}");
                    break;
            }
        }
    }

    /// <summary>
    /// 設定集合資料
    /// </summary>
    private void SetCollectionData(CollectionDataModel data)
    {
        for (int i = 1; i <= _ws.Cells.MaxRow; i++)
        {
            int j = 0; // 用於計算欄位的偏移量
            // 取得 A 欄位的值
            string valueA = _ws.GetValue($"A{i}").Trim();
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            switch (valueA)
            {
                case "DESCRIPTION":
                    j = i + 4;
                    if (!string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(_ws.GetValue($"E{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(_ws.GetValue($"I{j}")))
                                break;
                            data.GoodsTable ??= [];
                            data.GoodsTable.Add(new GoodsItem()
                            {
                                SONo = _ws.GetValue($"E{j}"),
                                InvoiceNo = _ws.GetValue($"I{j}"),
                                NetWeight = _ws.GetValue($"M{j}"),
                                GrossWeight = _ws.GetValue($"Q{j}"),
                                Quantity = _ws.GetValue($"U{j}")
                            });
                            j++;
                        }
                    }
                    break;
                case "TIME LOG":
                    j = i;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                        string.IsNullOrEmpty(_ws.GetValue($"L{j}")))
                            break;
                        //Console.WriteLine($"{GetValue($"E{j}")} {GetValue($"L{j}")}");
                        data.TimeLogTable ??= [];
                        data.TimeLogTable.Add(new TimeLogItem()
                        {
                            Event = _ws.GetValue($"E{j}"),
                            Data = _ws.GetValue($"L{j}")
                        });
                        j++;
                    }
                    break;
                case "INSPECTION":
                    j = i + 4;
                    if (!string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(_ws.GetValue($"I{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(_ws.GetValue($"I{j}")))
                                break;
                            data.InspectionTable ??= [];
                            data.InspectionTable.Add(new InspectionItem()
                            {
                                CoilNo = _ws.GetValue($"E{j}"),
                                Findings = _ws.GetValue($"I{j}"),
                                PhotoNo = _ws.GetValue($"U{j}")
                            });
                            j++;
                        }
                    }
                    break;
                case "QUANTITY":
                    j = i + 3;
                    if (!string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(_ws.GetValue($"J{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(_ws.GetValue($"I{j}")))
                                break;
                            data.QuantityTable ??= [];
                            data.QuantityTable.Add(new QuantityItem()
                            {
                                ContainerNo = _ws.GetValue($"E{j}"),
                                QuantityLoaded = _ws.GetValue($"J{j}"),
                                ShippingSealNo = _ws.GetValue($"Q{j}")
                            });
                            j++;
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 其他資料處理
    /// </summary>
    public void OtherDataProcessing(MainDataModel model)
    {
        // 如果 GoodsTable 有資料
        if (model.CollectionData.GoodsTable.Count != 0)
        {
            // 取出最後一筆資料
            GoodsItem lastItem = model.CollectionData.GoodsTable.Last();
            // 設定一次性資料欄位 NetWeight、GrossWeight、Quantity
            model.SingleData.GoodsTotalNetWeight = lastItem.NetWeight;
            model.SingleData.GoodsTotalGrossWeight = lastItem.GrossWeight;
            model.SingleData.GoodsTotalQuantity = lastItem.Quantity;
            // 設定完成後移除
            model.CollectionData.GoodsTable.Remove(lastItem);
        }
    }
}
