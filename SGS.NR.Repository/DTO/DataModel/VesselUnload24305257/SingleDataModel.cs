#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24305257;

public record SingleDataModel
{
    public string HeaderService { get; set; }
    public string HeaderCompany { get; set; }
    public string HeaderNo { get; set; }
    public string ReportTitle { get; set; }
    public string DateLocation { get; set; }
    public string Goods { get; set; }
    public string Goods2 { get; set; }
    public string Goods3 { get; set; }
    public string Goods4 { get; set; }
    //public string Shipment { get; set; }
    public string Shipper { get; set; }
    public string Buyer { get; set; }
    public string Packing { get; set; }
    public string Marks { get; set; }
    public string Stowage { get; set; }
    public string Discharge { get; set; }
    public string QuantityDelivered { get; set; }
    public string Inspection { get; set; }
    public string Remarks { get; set; }
    public string Remarks2 { get; set; }
    public string Remarks3 { get; set; }
    public string FooterLeft { get; set; }
    public string FooterRight { get; set; }

    public string GoodsTotalWeight { get; set; }
    public string GoodsTotalWeightUnit { get; set; }
    //public string GoodsTotalGrossWeight { get; set; }
    //public string GoodsTotalGrossWeightUnit { get; set; }
    public string GoodsTotalQuantity { get; set; }
    public string GoodsTotalQuantityUnit { get; set; }
}
