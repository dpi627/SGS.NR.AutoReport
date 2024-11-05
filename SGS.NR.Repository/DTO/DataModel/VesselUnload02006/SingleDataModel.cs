#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload02006;

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
    //public string Shipment { get; set; }
    public string Shipper { get; set; }
    public string Trader { get; set; }
    public string Buyer { get; set; }
    public string MarksPacking { get; set; }
    public string Stowage { get; set; }
    public string Discharge { get; set; }
    //public string General { get; set; }
    //public string HatchCover { get; set; }
    //public string CargoHold { get; set; }
    //public string GoDown { get; set; }
    //public string Loading { get; set; }
    public string Inspection { get; set; }
    public string QuantityDelivered { get; set; }
    //public string Stowage2 { get; set; }
    //public string Stowage3 { get; set; }
    public string Remarks { get; set; }
    public string Remarks2 { get; set; }
    public string Remarks3 { get; set; }
    public string FooterLeft { get; set; }
    public string FooterRight { get; set; }

    //public string GoodsTotalNetWeight { get; set; }
    //public string GoodsTotalNetWeightUnit { get; set; }
    //public string GoodsTotalGrossWeight { get; set; }
    //public string GoodsTotalGrossWeightUnit { get; set; }
    //public string GoodsTotalQuantity { get; set; }
    //public string GoodsTotalQuantityUnit { get; set; }
}
