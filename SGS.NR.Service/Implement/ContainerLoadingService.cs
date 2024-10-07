using Aspose.Words;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.ContainerLoading;
using SGS.NR.Repository.Interface;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel;
using SGS.NR.Service.Extensions;
using SGS.NR.Service.Interface;
using System.Data;

namespace SGS.NR.Service.Implement;

public class ContainerLoadingService(
    ILogger<ContainerLoadingService> logger,
    IMapper mapper,
    IContainerLoadingRepository repo
    ) : BaseService, IContainerLoadingService
{
    public ContainerLoadingResultModel GetDraft(ContainerLoadingInfo info)
    {
        logger.LogInformation("{MethodName} start with {@Info}", nameof(GetDraft), info);
        var condition = mapper.Map<ContainerLoadingInfo, ContainerLoadingCondition>(info);
        var data = repo.Read(condition);
        var result = Export(info, data);
        logger.LogInformation("{MethodName} end with {@Result}", nameof(GetDraft), result);
        return result;
    }

    private ContainerLoadingResultModel Export(ContainerLoadingInfo info, MainDataModel model)
    {
        ContainerLoadingResultModel result = new()
        {
            FilePath = info.TargetPath
        };

        try
        {
            // 載入範本
            Document doc = new(info.TemplatePath);

            // 準備一次性資料
            var single = model.SingleData.ToDictionary();
            // 填充一次性資料 (使用郵件合併功能)
            doc.MailMerge.Execute([.. single.Keys], [.. single.Values]);

            // 準備集合資料 (表格類型，需跑迴圈設定，版本關係只能使用 DataTable)
            DataTable? tableGoods = model.CollectionData.GoodsTable.ToDataTable();
            DataTable? tableTimeLog = model.CollectionData.TimeLogTable.ToDataTable();
            DataTable? tableQuantity = model.CollectionData.QuantityTable.ToDataTable();
            DataTable? tableInspection = model.CollectionData.InspectionTable.ToDataTable();

            DataSet ds = new();
            ds.Tables.Add(tableGoods);
            ds.Tables.Add(tableTimeLog);
            ds.Tables.Add(tableQuantity);
            ds.Tables.Add(tableInspection);

            // 填充集合資料 (使用郵件合併功能)
            doc.MailMerge.ExecuteWithRegions(ds);

            // 如果集合為空，移除範本中資料列
            if (tableGoods.Rows.Count == 0)
                doc.RemoveRowsByBookmark(nameof(tableGoods));
            if (tableInspection.Rows.Count == 0)
                doc.RemoveRowsByBookmark(nameof(tableInspection));
            if (tableQuantity.Rows.Count == 0)
                doc.RemoveRowsByBookmark(nameof(tableQuantity));

            // 保存結果文件
            doc.Save(info.TargetPath);
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = ex.Message;
            logger.LogError(ex, "Export error");
        }

        return result;
    }
}
