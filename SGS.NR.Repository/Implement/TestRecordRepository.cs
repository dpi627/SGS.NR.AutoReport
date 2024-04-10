using MiniExcelLibs;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;
using SGS.NR.Repository.Interface;

namespace SGS.NR.Repository.Implement
{
    internal class TestRecordRepository : ITestRecordRepository
    {
        /// <summary>
        /// 讀取測試記錄資料
        /// </summary>
        /// <param name="condition">查詢條件</param>
        /// <returns>測試記錄資料模型的集合</returns>
        public IEnumerable<TestRecordDataModel> Read(TestRecordCondition condition)
        {
            return MiniExcel
                .Query<TestRecordDataModel>(condition.ImportPath)
                .Where(x => !string.IsNullOrWhiteSpace(x.TestItem));
        }
    }
}
