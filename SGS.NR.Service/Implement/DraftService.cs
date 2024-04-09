using MiniSoftware;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel;
using SGS.NR.Service.Interface;

namespace SGS.NR.Service.Implement
{
    public class DraftService : IDraftService
    {
        public DraftResultModel Save(DraftInfo info)
        {
            MiniWord.SaveAsByTemplate(
                info.ExportPath,
                info.TemplatePath,
                info.Data
                );
            return new DraftResultModel();
        }
    }
}
