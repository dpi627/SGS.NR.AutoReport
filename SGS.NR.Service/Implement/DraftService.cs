using AutoMapper;
using MiniSoftware;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;
using SGS.NR.Repository.Interface;
using SGS.NR.Service.DTO;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel;
using SGS.NR.Service.Interface;
using SGS.NR.Util.Extension;

namespace SGS.NR.Service.Implement
{
    public class DraftService(
            IMapper mapper,
            ITestRecordRepository repo
            ) : IDraftService
    {
        /// <summary>
        /// 儲存草稿
        /// </summary>
        /// <param name="info">草稿資訊</param>
        /// <returns>草稿結果模型</returns>
        public DraftResultModel Save(DraftInfo info)
        {
            // 建立一次性資料
            var onceData = new ReportModel
            {
                Title = "Test Report",
                Date = DateTime.Now.ToString("yyyy-MM-dd")
            };
            // 從 Excel 中讀取集合資料（測試記錄）
            IEnumerable<TestRecordDataModel>? loopData = repo.Read(mapper.Map<DraftInfo, TestRecordCondition>(info));
            // 組合資料
            var data = CombineData(onceData, loopData);

            MiniWord.SaveAsByTemplate(
                info.ExportPath,
                info.TemplatePath,
                data
                );

            return new DraftResultModel();
        }

        /// <summary>
        /// 組合一次性資料與集合資料，並轉換為字典
        /// </summary>
        /// <typeparam name="TSingleton">單一物件類型</typeparam>
        /// <typeparam name="TCollection">集合物件類型</typeparam>
        /// <param name="Singleton">單一物件，儲存一次性資料</param>
        /// <param name="Collection">集合物件，儲存集合資料，通常以表格呈現</param>
        /// <param name="CollectionName">集合名稱，預設 "Loop"</param>
        /// <returns>結合後的字典</returns>
        private IDictionary<string, object> CombineData<TSingleton, TCollection>(
            TSingleton Singleton,
            IEnumerable<TCollection> Collection,
            string CollectionName = "Loop")
        {
            IDictionary<string, object>? result = Singleton.ToDict();
            result.Add(CollectionName, Collection.ToDict());
            return result;
        }
    }
}
