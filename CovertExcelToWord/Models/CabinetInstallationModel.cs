namespace CovertExcelToWord.Models;

public record CabinetInstallationModel
{
    public string? HeaderService { get; set; }
    public string? HeaderCompany { get; set; }
    public string? HeaderNo { get; set; }
    public string? ReportTitle { get; set; }
    public string? DateLocation { get; set; }
    public string? Goods { get; set; }
    public string? Description { get; set; }
    public string? Shipment { get; set; }
    public string? Shipper { get; set; }
    public string? Buyer { get; set; }
    public string? Packing { get; set; }
    public string? Marks { get; set; }
    public string? Inspection1 { get; set; }
    public string? Inspection2 { get; set; }
    public string? Quantity { get; set; }
    public string? Stowage { get; set; }
    public string? Remarks { get; set; }
    public string? FooterLeft { get; set; }
    public string? FooterRight { get; set; }

    public string? GoodsTotalNetWeight { get; set; }
    public string? GoodsTotalGrossWeight { get; set; }
    public string? GoodsTotalQuantity { get; set; }
}

public record CollectionData()
{
    public List<GoodsItem>? GoodsTable { get; set; } // 假設 GoodsTable 為一個表格
    public List<TimeLogItem>? TimeLogTable { get; set; } // 假設 TimeLogTable 為一個表格
    public List<InspectionItem>? InspectionTable { get; set; } // 假設 InspectionTable 為一個表格
    public List<QuantityItem>? QuantityTable { get; set; } // 假設 QuantityTable 為一個表格
}

// 額外表格類別的簡單範例
public record GoodsItem
{
    public string? SONo { get; set; }
    public string? InvoiceNo { get; set; }
    public string? NetWeight { get; set; }
    public string? GrossWeight { get; set; }
    public string? Quantity { get; set; }
}

public record TimeLogItem
{
    public string? Event { get; set; }
    public string? Data { get; set; }
}

public record InspectionItem
{
    public string? CoilNo { get; set; }
    public string? Findings { get; set; }
    public string? PhotoNo { get; set; }
}

public record QuantityItem
{
    public string? ContainerNo { get; set; }
    public string? QuantityLoaded { get; set; }
    public string? ShippingSealNo { get; set; }
}