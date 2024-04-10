using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;

namespace SGS.NR.Repository.Interface
{
    public interface ITestRecordRepository
    {
        public IEnumerable<TestRecordDataModel> Read(TestRecordCondition condition);
    }
}
