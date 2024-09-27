#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.ContainerLoading;

public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
    public List<InspectionItem> InspectionTable { get; set; }
    public List<QuantityItem> QuantityTable { get; set; }
}

public record GoodsItem
{
    public string SONo { get; set; }
    public string InvoiceNo { get; set; }
    public string NetWeight { get; set; }
    public string GrossWeight { get; set; }
    public string Quantity { get; set; }
}

public record TimeLogItem
{
    public string Event { get; set; }
    public string Data { get; set; }
}

public record InspectionItem
{
    public string CoilNo { get; set; }
    public string Findings { get; set; }
    public string PhotoNo { get; set; }
}

public record QuantityItem
{
    public string? ContainerNo { get; set; }
    public string? QuantityLoaded { get; set; }
    public string? ShippingSealNo { get; set; }
}