using System.Drawing;

namespace SGS.NR.Repository.DTO.DataModel.ContainerLoading;

public record MainDataModel
{
    public MainDataModel()
    {
        this.SingleData = new SingleDataModel();
        this.CollectionData = new CollectionDataModel();
    }

    public SingleDataModel SingleData { get; set; }
    public CollectionDataModel CollectionData { get; set; }
}
