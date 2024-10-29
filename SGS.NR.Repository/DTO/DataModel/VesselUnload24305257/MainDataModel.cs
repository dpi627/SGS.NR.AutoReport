#nullable disable

namespace SGS.NR.Repository.DTO.DataModel.VesselUnload24305257;

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
