#nullable disable

namespace SGS.NR.Service.DTO.ResultModel.ContrainerLoading;

public record MainResultModel
{
    public MainResultModel()
    {
        this.SingleData = new SingleResultModel();
        this.CollectionData = new CollectionResultModel();
    }

    public SingleResultModel SingleData { get; set; }
    public CollectionResultModel CollectionData { get; set; }
}