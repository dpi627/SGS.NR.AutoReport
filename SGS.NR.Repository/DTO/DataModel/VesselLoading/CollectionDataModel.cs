﻿#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselLoading;

public record CollectionDataModel
{
    public List<GoodsItem> GoodsTable { get; set; }
    public List<TimeLogItem> TimeLogTable { get; set; }
    public List<InspectionItem> InspectionTable { get; set; }
    public List<ShipItem> ShipTable { get; set; }
}

public record GoodsItem
{
    public string InvoiceNo { get; set; }
    public string SO { get; set; }
    public string LotColour { get; set; }
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

public record InspectionItem
{
    public string CoilNo { get; set; }
    public string Findings { get; set; }
    public string PhotoNo { get; set; }
}

public record ShipItem
{
    public string Event { get; set; }
    public string Data { get; set; }
}