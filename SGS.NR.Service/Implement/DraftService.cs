using AutoMapper;
using MiniSoftware;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel;
using SGS.NR.Repository.Interface;
using SGS.NR.Service.DTO;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel;
using SGS.NR.Service.Interface;
using SGS.NR.Util.Helper;

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
            var data = ReportHelper.CombineData(onceData, loopData);

            MiniWord.SaveAsByTemplate(
                info.ExportPath,
                info.TemplatePath,
                data
                );

            return new DraftResultModel();
        }
    }
}
