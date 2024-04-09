using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;
using SGS.NR.Repository.Interface;

namespace SGS.NR.Repository.Implement
{
    public class DraftRepository : IDraftRepository
    {
        public IEnumerable<DraftDataModel> GetDraftData(DraftCondition condition)
        {
            throw new NotImplementedException();
        }
    }
}
