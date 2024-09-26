namespace SGS.NR.Repository.DTO.DataModel.ContainerLoading;

public record MainDataModel
{
    public SingleDataModel SingleData { get; set; }
    public CollectionDataModel CollectionData { get; set; }
}
