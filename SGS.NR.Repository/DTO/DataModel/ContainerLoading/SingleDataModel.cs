#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.ContainerLoading
{
    public record SingleDataModel
    {
        public string HeaderService { get; set; }
        public string HeaderCompany { get; set; }
        public string HeaderNo { get; set; }
        public string ReportTitle { get; set; }
        public string DateLocation { get; set; }
        public string Goods { get; set; }
        public string Description { get; set; }
        public string Shipment { get; set; }
        public string Shipper { get; set; }
        public string Buyer { get; set; }
        public string Packing { get; set; }
        public string Marks { get; set; }
        public string Inspection1 { get; set; }
        public string Inspection2 { get; set; }
        public string Quantity { get; set; }
        public string Stowage { get; set; }
        public string Remarks { get; set; }
        public string FooterLeft { get; set; }
        public string FooterRight { get; set; }

        public string GoodsTotalNetWeight { get; set; }
        public string GoodsTotalGrossWeight { get; set; }
        public string GoodsTotalQuantity { get; set; }
    }
}
