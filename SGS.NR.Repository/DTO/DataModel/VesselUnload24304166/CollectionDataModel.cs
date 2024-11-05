#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24304166;


public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
}

public record GoodsItem
{
    public string Grade { get; set; }
    public string Size { get; set; }
    public string SizeUnit { get; set; }
    public string NetWeight { get; set; }
    public string NetWeightUnit { get; set; }
    public string GrossWeight { get; set; }
    public string GrossWeightUnit { get; set; }
    public string Package { get; set; }
    public string PackageUnit { get; set; }
}

public record TimeLogItem
{
    public string Event { get; set; }
    public string Data { get; set; }
}
