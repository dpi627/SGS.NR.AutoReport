namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24301715;

public record MainDataModel
{
    public MainDataModel()
    {
        SingleData = new SingleDataModel();
        CollectionData = new CollectionDataModel();
    }

    public SingleDataModel SingleData { get; set; }
    public CollectionDataModel CollectionData { get; set; }
}
