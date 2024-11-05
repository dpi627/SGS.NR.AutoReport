using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel;

namespace SGS.NR.Service.Interface;

public interface IContainerLoadingService
{
    public ContainerLoadingResultModel GetDraft(ContainerLoadingInfo info);
}
