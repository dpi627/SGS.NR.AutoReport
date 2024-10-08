using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.VesselLoading;

namespace SGS.NR.Repository.Interface;

public interface IVesselLoadingRepository
{
    public MainDataModel Read(VesselLoadingCondition condition);
}
