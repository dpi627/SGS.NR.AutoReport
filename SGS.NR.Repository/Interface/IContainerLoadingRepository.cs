using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.ContainerLoading;

namespace SGS.NR.Repository.Interface;

public interface IContainerLoadingRepository
{
    public MainDataModel Read(ContainerLoadingCondition condition);
}
