using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;

namespace SGS.NR.Repository.Interface
{
    public interface IDraftRepository
    {
        public IEnumerable<DraftDataModel> GetDraftData(DraftCondition condition);
    }
}
