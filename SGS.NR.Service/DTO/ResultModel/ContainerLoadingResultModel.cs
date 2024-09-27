﻿#nullable disable

namespace SGS.NR.Service.DTO.ResultModel;

public record ContainerLoadingResultModel : BaseResultModel
{
    public string FilePath { get; set; }
    public string FileName => Path.GetFileName(this.FilePath);
}
