#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload02006;

public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
    public List<QuantityDelivered> ShipTable { get; set; }
}

public record GoodsItem
{
    public string Grade { get; set; }
    public string Size { get; set; }
    public string SizeUnit { get; set; }
    public string Weight { get; set; }
    public string WeightUnit { get; set; }
    public string Package { get; set; }
    public string PackageUnit { get; set; }
}

public record TimeLogItem
{
    public string Event { get; set; }
    public string Data { get; set; }
    public string Event2 { get; set; }
    public string Data2 { get; set; }
}

public record QuantityDelivered
{
    public string Event { get; set; }
    public string Data { get; set; }
}