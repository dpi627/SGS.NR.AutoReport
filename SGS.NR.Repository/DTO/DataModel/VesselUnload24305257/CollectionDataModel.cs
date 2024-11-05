#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24305257;

public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
    public List<QuantityDeliveredItem> QuantityTable { get; set; }
    public List<InspectionItem> InspectionTable { get; set; }
}

public record GoodsItem
{
    public string BLNo { get; set; }
    public string LCNo { get; set; }
    public string Weight { get; set; }
    public string WeightUnit { get; set; }
    public string Quantity { get; set; }
    public string QuantityUnit { get; set; }
}

public record TimeLogItem
{
    public string Event { get; set; }
    public string Data { get; set; }
}

public record QuantityDeliveredItem
{
    public string BLNo { get; set; }
    public string S08A { get; set; }
    public string S08AB { get; set; }
    public string Total { get; set; }
}

public record InspectionItem
{
    public string HeatNo { get; set; }
    public string PartedSegments { get; set; }
    public string PartedLength { get; set; }
}
