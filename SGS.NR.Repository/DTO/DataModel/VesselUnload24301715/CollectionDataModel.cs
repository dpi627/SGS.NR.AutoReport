#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24301715;

public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
    public List<QuantityDelivered> ShipTable { get; set; }
}

public record GoodsItem
{
    public string Spec { get; set; }
    public string Size { get; set; }
    public string SizeUnit { get; set; }
    public string NetWeight { get; set; }
    public string NetWeightUnit { get; set; }
    public string GrossWeight { get; set; }
    public string GrossWeightUnit { get; set; }
    public string Quantity { get; set; }
    public string QuantityUnit { get; set; }
}

public record TimeLogItem
{
    public string Event { get; set; }
    public string Data { get; set; }
}

public record QuantityDelivered
{
    public string Event { get; set; }
    public string Data { get; set; }
}