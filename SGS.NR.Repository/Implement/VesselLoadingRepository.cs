using Aspose.Cells;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.VesselLoading;
using SGS.NR.Repository.Extension;
using SGS.NR.Repository.Interface;
using System.Globalization;

namespace SGS.NR.Repository.Implement;

public class VesselLoadingRepository : BaseRepository, IVesselLoadingRepository
{
    private Workbook _wb;
    private Worksheet _ws;
    // subtitle 請注意移除空白與換行，全部大寫英文相連
    // 資料讀取時候也必須記得取代 " " 與 "\n"
    private readonly List<string> _subtitles = [
        "GOODS","DESCRIPTION","SHIPMENT","SHIPPER","BUYER","PACKING","MARKS",
        "SHIP’SPARTICULAR", "GENERAL", "HATCHCOVER", "CARGOHOLD", "GODOWN", "LOADING",
        "TIMELOG","INSPECTION","QUANTITYLOADED","STOWAGE","REMARKS"
        ];
    private readonly MainDataModel model = new();

    public MainDataModel Read(VesselLoadingCondition condition)
    {
        // 取得 Excel
        _wb = new(condition.SourcePath);
        // 取得工作表
        _ws = _wb.Worksheets[condition.SheetName];
        // 設定一次性或固定資料
        SetSingleData(model.SingleData);
        // 設定集合(迴圈)資料
        SetCollectionData(model.CollectionData);
        // 其他資料處理
        OtherDataProcessing();

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
        data.FooterRight = "SGS International Services SA, Taiwan Branch";

        //不固定位置之資料，只能在迴圈期間抓取
        string valueA; //A欄資料


        for (int i = 1; i <= _ws.Cells.MaxRow; i++)
        {
            valueA = _ws.GetValue($"A{i}")
                .Replace(" ", "")
                .Replace("\n", "");
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA)) // || !_subtitles.Contains(valueA))
                continue;
            Console.WriteLine(valueA);
        }

        for (int i = 1; i <= _ws.Cells.MaxRow; i++)
        {
            // Excel 取出的值必須移除換行符號與空白
            valueA = _ws.GetValue($"A{i}")
                .Replace(" ", "")
                .Replace("\n", "");
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            // case 比對項目必須與上方 subtitle 吻合
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
                case "GENERAL":
                    data.General = _ws.GetValue($"E{i}");
                    break;
                case "HATCHCOVER":
                    data.HatchCover = _ws.GetValue($"E{i}");
                    break;
                case "CARGOHOLD":
                    data.CargoHold = _ws.GetValue($"E{i}");
                    break;
                case "GODOWN":
                    data.GoDown = _ws.GetValue($"E{i}");
                    break;
                case "LOADING":
                    data.Loading = _ws.GetValue($"E{i}");
                    break;
                case "INSPECTION":
                    data.Inspection = _ws.GetValue($"E{i}");
                    break;
                case "QUANTITYLOADED":
                    data.QuantityLoaded = _ws.GetValue($"E{i}");
                    break;
                case "STOWAGE":
                    data.Stowage = _ws.GetValue($"E{i}");
                    data.Stowage2 = _ws.GetValue($"E{i + 2}");
                    data.Stowage3 = _ws.GetValue($"E{i + 4}");
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
            string valueA = _ws.GetValue($"A{i}")
                .Replace(" ", "")
                .Replace("\n", "");
            // 如果 A 欄位的值為空或不存在 _subtitles，則跳過
            if (string.IsNullOrEmpty(valueA) || !_subtitles.Contains(valueA))
                continue;

            switch (valueA)
            {
                case "DESCRIPTION":
                    j = i + 4;
                    if (!string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                    !string.IsNullOrEmpty(_ws.GetValue($"H{j}")))
                    {
                        while (true)
                        {
                            if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                            string.IsNullOrEmpty(_ws.GetValue($"H{j}")))
                                break;
                            data.GoodsTable ??= [];
                            data.GoodsTable.Add(new GoodsItem()
                            {
                                InvoiceNo = _ws.GetValue($"E{j}"),
                                SO = _ws.GetValue($"H{j}"),
                                LotColour = _ws.GetValue($"K{j}"),
                                NetWeight = _ws.GetValue($"N{j}"),
                                NetWeightUnit = _ws.GetValue($"P{j}"),
                                GrossWeight = _ws.GetValue($"Q{j}"),
                                GrossWeightUnit = _ws.GetValue($"S{j}"),
                                Quantity = _ws.GetValue($"T{j}"),
                                QuantityUnit = _ws.GetValue($"U{j}")
                            });
                            j++;
                        }
                    }
                    break;
                case "SHIP’SPARTICULAR":
                    j = i;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                        string.IsNullOrEmpty(_ws.GetValue($"P{j}")))
                            break;
                        data.ShipTable ??= [];
                        data.ShipTable.Add(new ShipItem()
                        {
                            Event = _ws.GetValue($"E{j}"),
                            Data = _ws.GetValue($"P{j}")
                        });
                        j++;
                    }
                    break;
                case "TIMELOG":
                    j = i;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(_ws.GetValue($"E{j}")) &&
                        string.IsNullOrEmpty(_ws.GetValue($"M{j}")))
                            break;
                        data.TimeLogTable ??= [];
                        data.TimeLogTable.Add(new TimeLogItem()
                        {
                            Event = _ws.GetValue($"E{j}"),
                            Data = _ws.GetValue($"M{j}")
                        });
                        j++;
                    }
                    break;
                case "INSPECTION":
                    j = i + 3;
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
            }
        }
    }

    /// <summary>
    /// 其他資料處理
    /// </summary>
    public void OtherDataProcessing()
    {
        // 如果 GoodsTable 有資料
        if (model.CollectionData.GoodsTable.Count != 0)
        {
            // 取出最後一筆資料
            GoodsItem lastItem = model.CollectionData.GoodsTable.Last();
            // 設定一次性資料欄位 NetWeight、GrossWeight、Quantity
            model.SingleData.GoodsTotalNetWeight = lastItem.NetWeight;
            model.SingleData.GoodsTotalNetWeightUnit = lastItem.NetWeightUnit;
            model.SingleData.GoodsTotalGrossWeight = lastItem.GrossWeight;
            model.SingleData.GoodsTotalGrossWeightUnit = lastItem.GrossWeightUnit;
            model.SingleData.GoodsTotalQuantity = lastItem.Quantity;
            model.SingleData.GoodsTotalQuantityUnit = lastItem.QuantityUnit;
            // 設定完成後移除
            model.CollectionData.GoodsTable.Remove(lastItem);
        }
    }
}