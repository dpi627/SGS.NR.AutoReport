#nullable disable

namespace SGS.NR.Service.DTO.ResultModel.ContrainerLoading;


public record CollectionResultModel
{
    public List<GoodsItemResultModel> GoodsTable { get; set; }
    public List<TimeLogItemResultModel> TimeLogTable { get; set; }
    public List<InspectionItemResultModel> InspectionTable { get; set; }
    public List<QuantityItemResultModel> QuantityTable { get; set; }
}

public record GoodsItemResultModel
{
    public string SONo { get; set; }
    public string InvoiceNo { get; set; }
    public string NetWeight { get; set; }
    public string GrossWeight { get; set; }
    public string Quantity { get; set; }
}

public record TimeLogItemResultModel
{
    public string Event { get; set; }
    public string Data { get; set; }
}

public record InspectionItemResultModel
{
    public string CoilNo { get; set; }
    public string Findings { get; set; }
    public string PhotoNo { get; set; }
}

public record QuantityItemResultModel
{
    public string? ContainerNo { get; set; }
    public string? QuantityLoaded { get; set; }
    public string? ShippingSealNo { get; set; }
}
